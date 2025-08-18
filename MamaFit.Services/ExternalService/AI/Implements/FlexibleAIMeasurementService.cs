using System.Text.Json;
using System.Text.Json.Serialization;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Services.ExternalService.AI.Interface;
using MamaFit.Services.ExternalService.AI.Prompts;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Service.Caculator;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MamaFit.Services.ExternalService.AI.Implements;

public class FlexibleAIMeasurementService : IAIMeasurementCalculationService
{
    private readonly ILogger<FlexibleAIMeasurementService> _logger;
    private readonly IConfiguration _configuration;
    private readonly GroqService _groqService;
    private readonly OllamaService _ollamaService;
    private readonly IBodyGrowthCalculator _calculator;
    private readonly ICacheService _cache;
    private ILLMProvider? _activeProvider;

    public FlexibleAIMeasurementService(
        ILogger<FlexibleAIMeasurementService> logger,
        IConfiguration configuration,
        GroqService groqService,
        OllamaService ollamaService,
        IBodyGrowthCalculator calculator,
        ICacheService cache)
    {
        _logger = logger;
        _configuration = configuration;
        _groqService = groqService;
        _ollamaService = ollamaService;
        _calculator = calculator;
        _cache = cache;

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

    private bool HasCurrentCore(MeasurementCreateDto? input) =>
        input != null && input.Weight > 0 && input.Bust > 0 && input.Waist > 0 && input.Hip > 0;

    private bool HasDiaryCore(MeasurementDiaryDto diary) =>
        diary.Weight > 0 && diary.Bust > 0 && diary.Waist > 0 && diary.Hip > 0;

    private (float Weight, float Bust, float Waist, float Hip)? PickCoreToLock(
        MeasurementDiaryDto diary, MeasurementCreateDto? input)
    {
        if (HasCurrentCore(input))
            return (input!.Weight, input.Bust, input.Waist, input.Hip);
        if (HasDiaryCore(diary))
            return (diary.Weight, diary.Bust, diary.Waist, diary.Hip);
        return null;
    }

    public async Task<MeasurementDto> CalculateMeasurementsAsync(
        MeasurementDiaryDto diary,
        MeasurementCreateDto? currentInput,
        Measurement? lastMeasurement,
        int targetWeek)
    {
        MeasurementDto fallbackResult;
        try
        {
            var cacheKey = GenerateCacheKey(diary, currentInput, lastMeasurement, targetWeek);
            var cached = await _cache.GetAsync<MeasurementDto>(cacheKey);
            if (cached != null) return cached;

            if (_activeProvider == null) await InitializeProvider();

            string prompt = FlexibleMeasurementPrompts.GetFlexibleCalculationPrompt(
                diary, currentInput, lastMeasurement, targetWeek);

            MeasurementDto? measurements = null;
            if (_activeProvider != null)
            {
                try
                {
                    var resp = await _activeProvider.GenerateResponseAsync(prompt);
                    measurements = ParseAIResponse(resp);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error with provider: {_activeProvider?.GetProviderName()}");
                }
            }

            if (measurements == null)
            {
                fallbackResult = CalculateWithFallback(diary, currentInput, lastMeasurement, targetWeek);
                await CacheResult(cacheKey, fallbackResult);
                return fallbackResult;
            }

            var core = PickCoreToLock(diary, currentInput);
            bool lockCore = core.HasValue;

            if (lockCore)
            {
                measurements.Weight = core.Value.Weight;
                measurements.Bust = core.Value.Bust;
                measurements.Waist = core.Value.Waist;
                measurements.Hip = core.Value.Hip;
            }

            var validated = ApplySafetyValidations(measurements, diary, targetWeek, lockCoreManual: lockCore);
            await CacheResult(cacheKey, validated);
            return validated;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error in AI calculation");
            fallbackResult = CalculateWithFallback(diary, currentInput, lastMeasurement, targetWeek);
            await CacheResult(GenerateCacheKey(diary, currentInput, lastMeasurement, targetWeek), fallbackResult);
            return fallbackResult;
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
        int targetWeek,
        bool lockCoreManual = false)
    {
        measurements.WeekOfPregnancy = targetWeek;

        if (!lockCoreManual)
        {
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

            var maxBustIncrease = 20f;
            measurements.Bust = Math.Min(measurements.Bust, diary.Bust + maxBustIncrease);

            var maxHipIncrease = 15f;
            measurements.Hip = Math.Min(measurements.Hip, diary.Hip + maxHipIncrease);

            var maxWaistIncrease = targetWeek * 0.5f;
            measurements.Waist = Math.Min(measurements.Waist, diary.Waist + maxWaistIncrease);
        }

        var estimatedNeck = diary.Height / 5.5f;
        measurements.Neck = Math.Clamp(
            measurements.Neck,
            estimatedNeck * 0.9f,
            estimatedNeck + 2f
        );
        measurements.Stomach = Math.Max(measurements.Stomach, measurements.Waist + 5f);
        measurements.PantsWaist = Math.Max(measurements.PantsWaist, 50f);
        measurements.Coat = Math.Max(measurements.Coat, measurements.Bust);
        measurements.ChestAround = Math.Max(measurements.ChestAround, 60f);
        measurements.Thigh = Math.Max(measurements.Thigh, 30f);
        measurements.ShoulderWidth = Math.Max(measurements.ShoulderWidth, 30f);
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

    private string GenerateCacheKey(
        MeasurementDiaryDto diary,
        MeasurementCreateDto? currentInput,
        Measurement? lastMeasurement,
        int targetWeek)
    {
        var keyParts = new List<string>
        {
            "AI_Measurement",
            targetWeek.ToString(),
            diary.Height.ToString("F1"),
            diary.Weight.ToString("F1"),
            diary.Bust.ToString("F1"),
            diary.Waist.ToString("F1"),
            diary.Hip.ToString("F1")
        };

        if (currentInput != null)
        {
            keyParts.AddRange(new[]
            {
                currentInput.Weight.ToString("F1"),
                currentInput.Bust.ToString("F1"),
                currentInput.Waist.ToString("F1"),
                currentInput.Hip.ToString("F1")
            });
        }
        else
        {
            keyParts.AddRange(new[] { "0", "0", "0", "0" });
        }

        if (lastMeasurement != null)
        {
            keyParts.AddRange(new[]
            {
                lastMeasurement.WeekOfPregnancy.ToString(),
                lastMeasurement.Weight.ToString("F1"),
                lastMeasurement.Bust.ToString("F1"),
                lastMeasurement.Waist.ToString("F1"),
                lastMeasurement.Hip.ToString("F1")
            });
        }
        else
        {
            // Thêm giá trị mặc định nếu lastMeasurement là null
            keyParts.AddRange(new[] { "0", "0", "0", "0", "0" });
        }

        return string.Join("|", keyParts);
    }

    private async Task CacheResult(string cacheKey, MeasurementDto result)
    {
        try
        {
            var cacheExpiry = TimeSpan.FromDays(30);
            await _cache.SetAsync(cacheKey, result, cacheExpiry);
            _logger.LogDebug($"Cached measurement result for key: {cacheKey}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache measurement result");
        }
    }

    public async Task<bool> IsAvailable()
    {
        if (_activeProvider == null)
        {
            await InitializeProvider();
        }

        return _activeProvider != null;
    }
}