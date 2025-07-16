using MamaFit.BusinessObjects.DTO.SizeDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/size")]
public class SizeController : ControllerBase
{
    private readonly ISizeService _sizeService;
    
    public SizeController(ISizeService sizeService)
    {
        _sizeService = sizeService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _sizeService.GetAllAsync(index, pageSize, search);
        return Ok(new ResponseModel<PaginatedList<SizeDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all sizes successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] SizeRequestDto requestDto)
    {
        await _sizeService.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<string>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                null,
                "Created size successfully!"
            ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var size = await _sizeService.GetByIdAsync(id);
        return Ok(new ResponseModel<SizeDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            size,
            "Get size successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] SizeRequestDto requestDto)
    {
        await _sizeService.UpdateAsync(id, requestDto);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Updated size successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        await _sizeService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted size successfully!"
        ));
    }
}