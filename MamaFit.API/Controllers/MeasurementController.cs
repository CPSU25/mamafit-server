using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/measurement")]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService _service;

    public MeasurementController(IMeasurementService service)
    {
        _service = service;
    }

    [HttpPost("preview-measurement")]
    public async Task<IActionResult> Preview([FromBody] MeasurementCreateDto dto)
    {
        var result = await _service.GenerateMeasurementPreviewAsync(dto);
        return Ok(new ResponseModel<MeasurementDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            result,
            "Preview measurement successfully!"
        ));
    }
    
    [HttpPost("create-measurement")]
    public async Task<IActionResult> Create([FromBody] CreateMeasurementDto dto)
    {
        var result = await _service.CreateMeasurementAsync(dto);
        return Ok(new ResponseModel<MeasurementDto>(
            StatusCodes.Status201Created,
            ResponseCodeConstants.CREATED,
            result,
            "Measurement created successfully!"
        ));
    }
    
    [HttpPost("preview-diary")]
    public async Task<IActionResult> Preview([FromBody] MeasurementDiaryDto dto)
    {
        var result = await _service.GenerateMeasurementDiaryPreviewAsync(dto);
        return Ok(new ResponseModel<MeasurementDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            result,
            "Preview measurement successfully!"
        ));
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] MeasurementDiaryCreateRequest request)
    {
        var diaryId = await _service.CreateDiaryWithMeasurementAsync(request);
        return Ok(new ResponseModel<object>(
            StatusCodes.Status201Created,
            ResponseCodeConstants.CREATED,
            new { diaryId },
            "Measurement diary and measurement created successfully!"
        ));
    }
}
