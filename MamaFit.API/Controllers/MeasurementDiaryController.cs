using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/measurement-diary")]
public class MeasurementDiaryController : ControllerBase
{
    private readonly IMeasurementDiaryService _diaryService;

    public MeasurementDiaryController(IMeasurementDiaryService diaryService)
    {
        _diaryService = diaryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var diaries = await _diaryService.GetAllAsync(index, pageSize, search);
        return Ok( new ResponseModel<PaginatedList<MeasurementDiaryResponseDto>>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            diaries, "Get all measurement diaries successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var diary = await _diaryService.GetDiaryByIdAsync(id);
        return Ok(new ResponseModel<MeasurementDiaryResponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            diary,
            "Get measurement diary by ID successfully!"
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _diaryService.DeleteDiaryAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null,
            "Measurement diary deleted successfully!"
        ));
    }
}