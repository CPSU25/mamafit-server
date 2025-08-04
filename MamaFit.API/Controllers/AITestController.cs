using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.AI;
using MamaFit.Services.ExternalService.AI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/ai-test")]
[Authorize]
public class AITestController : ControllerBase
{
    private readonly IAIMeasurementCalculationService _aiService;
    private readonly ILogger<AITestController> _logger;

    public AITestController(
        IAIMeasurementCalculationService aiService,
        ILogger<AITestController> logger)
    {
        _aiService = aiService;
        _logger = logger;
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckAIStatus()
    {
        try
        {
            var isAvailable = await _aiService.IsAvailable();

            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                new
                {
                    aiAvailable = isAvailable,
                    timestamp = DateTime.UtcNow,
                    provider = isAvailable ? "Active" : "None"
                },
                "AI status retrieved successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking AI status");
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                new
                {
                    aiAvailable = false,
                    timestamp = DateTime.UtcNow,
                    error = ex.Message
                },
                "AI service unavailable"
            ));
        }
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> TestCalculation([FromBody] AITestRequest request)
    {
        try
        {
            var diaryDto = new MeasurementDiaryDto
            {
                Age = request.Age,
                Height = request.Height,
                Weight = request.PrePregnancyWeight,
                Bust = request.PrePregnancyBust,
                Waist = request.PrePregnancyWaist,
                Hip = request.PrePregnancyHip
            };

            var result = await _aiService.CalculateMeasurementsAsync(
                diaryDto,
                null,
                null,
                request.TargetWeek
            );

            return Ok(new ResponseModel<MeasurementDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "AI calculation successful"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test calculation failed");
            return StatusCode(500, new ResponseModel<object>(
                StatusCodes.Status500InternalServerError,
                ApiCodes.INTERNAL_SERVER_ERROR,
                null,
                $"AI calculation failed: {ex.Message}"
            ));
        }
    }

    [HttpPost("calculate-with-input")]
    public async Task<IActionResult> TestCalculationWithInput([FromBody] AITestWithInputRequest request)
    {
        try
        {
            var diaryDto = new MeasurementDiaryDto
            {
                Age = request.Age,
                Height = request.Height,
                Weight = request.PrePregnancyWeight,
                Bust = request.PrePregnancyBust,
                Waist = request.PrePregnancyWaist,
                Hip = request.PrePregnancyHip
            };

            var currentInput = new MeasurementCreateDto
            {
                Weight = request.CurrentWeight,
                Bust = request.CurrentBust,
                Waist = request.CurrentWaist,
                Hip = request.CurrentHip
            };

            var result = await _aiService.CalculateMeasurementsAsync(
                diaryDto,
                currentInput,
                null,
                request.TargetWeek
            );

            return Ok(new ResponseModel<MeasurementDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "AI calculation with input successful"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test calculation with input failed");
            return StatusCode(500, new ResponseModel<object>(
                StatusCodes.Status500InternalServerError,
                ApiCodes.INTERNAL_SERVER_ERROR,
                null,
                $"AI calculation failed: {ex.Message}"
            ));
        }
    }
}

public class AITestRequest
{
    public int Age { get; set; } = 28;
    public float Height { get; set; } = 165;
    public float PrePregnancyWeight { get; set; } = 60;
    public float PrePregnancyBust { get; set; } = 85;
    public float PrePregnancyWaist { get; set; } = 70;
    public float PrePregnancyHip { get; set; } = 90;
    public int TargetWeek { get; set; } = 20;
}

public class AITestWithInputRequest : AITestRequest
{
    public float CurrentWeight { get; set; } = 65;
    public float CurrentBust { get; set; } = 88;
    public float CurrentWaist { get; set; } = 75;
    public float CurrentHip { get; set; } = 93;
}