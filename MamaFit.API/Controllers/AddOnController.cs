using MamaFit.BusinessObjects.DTO.AddOnDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/add-on")]
public class AddOnController : ControllerBase
{
    private readonly IAddOnService _service;
    
    public AddOnController(IAddOnService service)
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
        var maternityDressServices = await _service.GetAllAsync(index, pageSize, search, sortBy);
        return Ok(new ResponseModel<PaginatedList<AddOnDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            maternityDressServices,
            "Get all maternity dress services successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddOnRequestDto requestDto)
    {
        await _service.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<string>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                null,
                "Created maternity dress service successfully!"
            ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var maternityDressService = await _service.GetByIdAsync(id);
        return Ok(new ResponseModel<AddOnDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            maternityDressService,
            "Get maternity dress service successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] AddOnRequestDto requestDto)
    {
        await _service.UpdateAsync(id, requestDto);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Updated maternity dress service successfully!"
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
            "Deleted maternity dress service successfully!"
        ));
    }
}