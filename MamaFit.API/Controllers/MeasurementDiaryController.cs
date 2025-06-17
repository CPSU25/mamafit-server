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
        [FromQuery] string? nameSearch = null)
    {
        var diaries = await _diaryService.GetAllAsync(index, pageSize, nameSearch);
        return Ok( new ResponseModel<PaginatedList<MeasurementDiaryResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            diaries, "Get all measurement diaries successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var diary = await _diaryService.GetDiaryByIdAsync(id, startDate, endDate);
        return Ok(new ResponseModel<DiaryWithMeasurementDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            diary,
            "Get measurement diary by ID successfully!"
        ));
    }
    
    [HttpGet("userId")]
    public async Task<IActionResult> GetByUserId(
        [FromQuery]string userId,
        [FromQuery]int index = 1,
        [FromQuery]int pageSize = 10,
        [FromQuery]string? nameSearch = null)
    {
        var diaries = await _diaryService.GetDiariesByUserIdAsync(index,pageSize, userId, nameSearch);
        return Ok(new ResponseModel<PaginatedList<MeasurementDiaryResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            diaries,
            "Get measurement diaries by user ID successfully!"
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _diaryService.DeleteDiaryAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Measurement diary deleted successfully!"
        ));
    }
}