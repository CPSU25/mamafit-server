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
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _service.GetAllMeasurementsAsync(index, pageSize, startDate, endDate);
        return Ok(new ResponseModel<PaginatedList<MeasurementResponseDto>>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            result,
            "Get all measurements successfully!"
        ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _service.GetMeasurementByIdAsync(id);
        return Ok(new ResponseModel<MeasurementResponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            result,
            "Get measurement successfully!"
        ));
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
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateMeasurementDto dto)
    {
        var result = await _service.UpdateMeasurementAsync(id, dto);
        return Ok(new ResponseModel<MeasurementDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            result,
            "Measurement updated successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var isDeleted = await _service.DeleteMeasurementAsync(id);
        if (!isDeleted)
        {
            return NotFound(new ResponseModel<string>(
                StatusCodes.Status404NotFound,
                ResponseCodeConstants.NOT_FOUND,
                null,
                "Measurement not found!"
            ));
        }
        
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null,
            "Measurement deleted successfully!"
        ));
    }
}
