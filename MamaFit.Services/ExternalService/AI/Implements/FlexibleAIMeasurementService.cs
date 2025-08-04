using System.Text.Json;
using System.Text.Json.Serialization;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Services.ExternalService.AI.Interface;
using MamaFit.Services.ExternalService.AI.Prompts;
using MamaFit.Services.Service.Caculator;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MamaFit.Services.ExternalService.AI.Implements;

public class FlexibleAIMeasurementService : IAIMeasurementCalculationService
{
    private readonly ILogger<FlexibleAIMeasurementService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly GroqService _groqService;
    private readonly OllamaService _ollamaService;
    private readonly IBodyGrowthCalculator _calculator;
    private ILLMProvider? _activeProvider;

    public FlexibleAIMeasurementService(
        ILogger<FlexibleAIMeasurementService> logger,
        IConfiguration configuration,
        IMemoryCache cache,
        GroqService groqService,
        OllamaService ollamaService,
        IBodyGrowthCalculator calculator)
    {
        _logger = logger;
        _configuration = configuration;
        _cache = cache;
        _groqService = groqService;
        _ollamaService = ollamaService;
        _calculator = calculator;

        Task.Run(async () => await InitializeProvider());
    }

    private async Task InitializeProvider()
    {
        try
        {
            if (_configuration.GetValue<bool>("AI:Providers:Groq:Enabled"))
            {
                try
                {
                    if (await _groqService.IsAvailable())
                    {
                        _activeProvider = _groqService;
                        _logger.LogInformation("Using Groq for AI calculations");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking Groq availability");
                }
            }

            if (_configuration.GetValue<bool>("AI:Providers:Ollama:Enabled"))
            {
                try
                {
                    if (await _ollamaService.IsAvailable())
                    {
                        _activeProvider = _ollamaService;
                        _logger.LogInformation("Using Ollama for AI calculations");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking Ollama availability");
                }
            }

            _logger.LogWarning("No available AI provider");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error initializing AI providers");
        }
    }

    public async Task<MeasurementDto> CalculateMeasurementsAsync(
        MeasurementDiaryDto diary,
        MeasurementCreateDto? currentInput,
        Measurement? lastMeasurement,
        int targetWeek)
    {
        try
        {
            if (_activeProvider == null)
            {
                await InitializeProvider();
                
                if (_activeProvider == null)
                {
                    return CalculateWithFallback(diary, currentInput, lastMeasurement, targetWeek);
                }
            }

            string? prompt = null;
            try
            {
                prompt = FlexibleMeasurementPrompts.GetFlexibleCalculationPrompt(
                    diary, currentInput, lastMeasurement, targetWeek);

                _logger.LogInformation($"Requesting AI calculation from {_activeProvider.GetProviderName()}");
                var response = await _activeProvider.GenerateResponseAsync(prompt);
                var measurements = ParseAIResponse(response);

                if (measurements != null)
                {
                    return ApplySafetyValidations(measurements, diary, targetWeek);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error with {_activeProvider?.GetProviderName() ?? "active provider"}");
                
                if (_activeProvider is GroqService &&
                    _configuration.GetValue<bool>("AI:Providers:Ollama:Enabled") &&
                    !string.IsNullOrEmpty(prompt))
                {
                    try
                    {
                        if (await _ollamaService.IsAvailable())
                        {
                            _activeProvider = _ollamaService;
                            
                            var retryResponse = await _ollamaService.GenerateResponseAsync(prompt);
                            var retryMeasurements = ParseAIResponse(retryResponse);

                            if (retryMeasurements != null)
                            {
                                return ApplySafetyValidations(retryMeasurements, diary, targetWeek);
                            }
                        }
                    }
                    catch (Exception ollamaEx)
                    {
                        _logger.LogError(ollamaEx, "Failed to use Ollama as fallback provider");
                    }
                }
            }
            
            return CalculateWithFallback(diary, currentInput, lastMeasurement, targetWeek);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error in AI calculation");
            return CalculateWithFallback(diary, currentInput, lastMeasurement, targetWeek);
        }
    }

    private MeasurementDto? ParseAIResponse(string response)
    {
        try
        {
            _logger.LogDebug($"Parsing AI response: {response}");

            // Try to extract JSON from response
            var jsonStart = response.IndexOf('{');
            var jsonEnd = response.LastIndexOf('}') + 1;

            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                var jsonStr = response.Substring(jsonStart, jsonEnd - jsonStart);

                // Clean up common issues
                jsonStr = jsonStr.Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("\t", "")
                    .Replace("\\\"", "\"");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var result = JsonSerializer.Deserialize<MeasurementDto>(jsonStr, options);

                if (result != null)
                {
                    _logger.LogInformation("Successfully parsed AI response");
                    return result;
                }
            }

            _logger.LogWarning("Could not find valid JSON in AI response");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to parse AI response: {response}");
            return null;
        }
    }

    private MeasurementDto ApplySafetyValidations(
        MeasurementDto measurements,
        MeasurementDiaryDto diary,
        int targetWeek)
    {
        // Weight safety check
        var bmi = diary.Weight / ((diary.Height / 100) * (diary.Height / 100));
        var maxTotalGain = GetMaxSafeWeightGain(bmi);
        var maxWeightForWeek = diary.Weight + (maxTotalGain * targetWeek / 40f);

        if (measurements.Weight > maxWeightForWeek)
        {
            _logger.LogWarning(
                $"AI suggested weight {measurements.Weight}kg exceeds safe limit, adjusting to {maxWeightForWeek}kg");
            measurements.Weight = maxWeightForWeek;
        }

        if (measurements.Weight < diary.Weight)
        {
            _logger.LogWarning("AI suggested weight loss during pregnancy, adjusting");
            measurements.Weight = diary.Weight;
        }

        // Basic proportion checks
        if (measurements.Waist < diary.Waist * 0.9f)
        {
            _logger.LogWarning("AI suggested unrealistic waist reduction");
            measurements.Waist = diary.Waist;
        }

        // Ensure all values are positive and reasonable
        measurements.Weight = Math.Max(measurements.Weight, 30f);
        measurements.Bust = Math.Max(measurements.Bust, 60f);
        measurements.Waist = Math.Max(measurements.Waist, 50f);
        measurements.Hip = Math.Max(measurements.Hip, 60f);
        measurements.Neck = Math.Max(measurements.Neck, 25f);
        measurements.Stomach = Math.Max(measurements.Stomach, measurements.Waist);
        measurements.PantsWaist = Math.Max(measurements.PantsWaist, 50f);
        measurements.Coat = Math.Max(measurements.Coat, measurements.Bust);
        measurements.ChestAround = Math.Max(measurements.ChestAround, 60f);
        measurements.Thigh = Math.Max(measurements.Thigh, 30f);
        measurements.ShoulderWidth = Math.Max(measurements.ShoulderWidth, 30f);
        measurements.SleeveLength = Math.Max(measurements.SleeveLength, 40f);
        measurements.DressLength = Math.Max(measurements.DressLength, 80f);
        measurements.LegLength = Math.Max(measurements.LegLength, 60f);

        return measurements;
    }

    private float GetMaxSafeWeightGain(float bmi)
    {
        // IOM guidelines for total pregnancy weight gain
        if (bmi < 18.5f) return 18f; // Underweight
        if (bmi < 25f) return 16f; // Normal
        if (bmi < 30f) return 11.5f; // Overweight
        return 9f; // Obese
    }

    private string GenerateCacheKey(int week, MeasurementCreateDto? input)
    {
        var inputHash = input != null ? $"{input.Weight:F1}{input.Bust:F1}{input.Waist:F1}{input.Hip:F1}" : "no-input";
        return $"ai-calc:{week}:{inputHash}";
    }

    private MeasurementDto CalculateWithFallback(
        MeasurementDiaryDto diary,
        MeasurementCreateDto? currentInput,
        Measurement? lastMeasurement,
        int targetWeek)
    {
        _logger.LogInformation("Using fallback calculator");

        // If manual input provided, use it
        if (currentInput != null && currentInput.Weight > 0)
        {
            return new MeasurementDto
            {
                WeekOfPregnancy = targetWeek,
                Weight = currentInput.Weight,
                Bust = currentInput.Bust,
                Waist = currentInput.Waist,
                Hip = currentInput.Hip,
                Neck = diary.Height / 5f,
                Stomach = currentInput.Waist + 5f,
                PantsWaist = currentInput.Waist - 5f,
                Coat = currentInput.Bust + 5f,
                ChestAround = currentInput.Bust + 3f,
                Thigh = (currentInput.Hip + 5f) / 2f,
                ShoulderWidth = diary.Height / 4.3f,
                SleeveLength = diary.Height / 6.4f,
                DressLength = diary.Height * 0.66f,
                LegLength = diary.Height * 0.48f
            };
        }

        // Use calculator with last measurement or diary
        float baseWeight = lastMeasurement?.Weight ?? diary.Weight;
        float baseBust = lastMeasurement?.Bust ?? diary.Bust;
        float baseWaist = lastMeasurement?.Waist ?? diary.Waist;
        float baseHip = lastMeasurement?.Hip ?? diary.Hip;
        int baseWeek = lastMeasurement?.WeekOfPregnancy ?? 0;

        var calculatedWeight = _calculator.CalculateWeight(baseWeight, baseWeek, targetWeek);
        var calculatedBust = _calculator.CalculateBust(baseBust, baseWeek, targetWeek);
        var calculatedWaist = _calculator.CalculateWaist(baseWaist, baseWeek, targetWeek);
        var calculatedHip = _calculator.CalculateHip(baseHip, baseWeek, targetWeek);

        return new MeasurementDto
        {
            WeekOfPregnancy = targetWeek,
            Weight = calculatedWeight,
            Bust = calculatedBust,
            Waist = calculatedWaist,
            Hip = calculatedHip,
            Neck = diary.Height / 5f,
            Stomach = calculatedWaist + 5f,
            PantsWaist = calculatedWaist - 5f,
            Coat = calculatedBust + 5f,
            ChestAround = calculatedBust + 3f,
            Thigh = (calculatedHip + 5f) / 2f,
            ShoulderWidth = diary.Height / 4.3f,
            SleeveLength = diary.Height / 6.4f,
            DressLength = diary.Height * 0.66f,
            LegLength = diary.Height * 0.48f
        };
    }

    public async Task<bool> IsAvailable()
    {
        if (_activeProvider == null)
        {
            // Try to reinitialize
            await InitializeProvider();
        }

        return _activeProvider != null;
    }
}