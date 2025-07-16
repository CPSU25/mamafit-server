using MamaFit.BusinessObjects.DTO.AddOnOptionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/add-on-options")]
public class AddOnOptionController : ControllerBase
{
    private readonly IAddOnOptionService _service;
    public AddOnOptionController(IAddOnOptionService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] EntitySortBy? sortBy = null)
    {
        var addOnOptions = await _service.GetAllAsync(index, pageSize, search, sortBy);
        return Ok(new ResponseModel<PaginatedList<AddOnOptionDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            addOnOptions,
            "Get all add-on options successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddOnOptionRequestDto requestDto)
    {
        await _service.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<string>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                null,
                "Created add-on option successfully!"
            ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var addOnOption = await _service.GetByIdAsync(id);
        return Ok(new ResponseModel<AddOnOptionDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            addOnOption,
            "Get add-on option successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] AddOnOptionRequestDto requestDto)
    {
        await _service.UpdateAsync(id, requestDto);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Updated add-on option successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted add-on option successfully!"
        ));
    }
}