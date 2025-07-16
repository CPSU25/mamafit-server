using MamaFit.BusinessObjects.DTO.PositionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/position")]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;
    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] EntitySortBy? sortBy = null)
    {
        var result = await _positionService.GetAllAsync(index, pageSize, search, sortBy);
        return Ok(new ResponseModel<PaginatedList<PositionDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all positions successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] PositionRequestDto requestDto)
    {
        await _positionService.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<string>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                null,
                "Created position successfully!"
            ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var position = await _positionService.GetByIdAsync(id);
        return Ok(new ResponseModel<PositionDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            position,
            "Get position successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] PositionRequestDto requestDto)
    {
        await _positionService.UpdateAsync(id, requestDto);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Updated position successfully!"
        ));
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        await _positionService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted position successfully!"
        ));
    }
}