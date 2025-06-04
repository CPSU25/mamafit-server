using MamaFit.BusinessObjects.DTO.MeasurementDiaryDto;
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
            diaries, null, "Get all measurement diaries successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var diary = await _diaryService.GetByIdAsync(id);
        return Ok(new ResponseModel<MeasurementDiaryResponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            diary, null, 
            "Get measurement diary by ID successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MeasurementDiaryRequestDto requestDto)
    {
         var diary = await _diaryService.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created, new ResponseModel<MeasurementDiaryResponseDto>(
            StatusCodes.Status201Created,
            ResponseCodeConstants.CREATED,
            diary, null,
            "Measurement diary created successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateMeasurementDiaryDto model)
    {
        var diary = await _diaryService.UpdateAsync(id, model);
        return Ok(new ResponseModel<MeasurementDiaryResponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            diary, null,
            "Measurement diary updated successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _diaryService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null, null,
            "Measurement diary deleted successfully!"
        ));
    }
}