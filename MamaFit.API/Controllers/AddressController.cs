using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10)
    {
        var addresses = await _addressService.GetAllAsync(index, pageSize);
        return Ok(new ResponseModel<PaginatedList<AddressResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            addresses,
            "Get all addresses successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var address = await _addressService.GetByIdAsync(id);
        return Ok(new ResponseModel<AddressResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            address,
            "Get address successfully!"
        ));
    }
    
    [Authorize]
    [HttpGet("by-token")]
    public async Task<IActionResult> GetByUser([FromHeader(Name = "Authorization")] string accessToken)
    {
        var addresses = await _addressService.GetByAccessTokenAsync(accessToken);
        return Ok(new ResponseModel<List<AddressResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            addresses,
            "Get addresses by user successfully!"
        ));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddressRequestDto requestDto,
        [FromHeader(Name = "Authorization")] string accessToken)
    {
        var addresses = await _addressService.CreateAsync(requestDto, accessToken);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<AddressResponseDto>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                addresses,
                "Created address successfully!"
            ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] AddressRequestDto requestDto)
    {
        await _addressService.UpdateAsync(id, requestDto);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Updated address successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _addressService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted address successfully!"
        ));
    }
}