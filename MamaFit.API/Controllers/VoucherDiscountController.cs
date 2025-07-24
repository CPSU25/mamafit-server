using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/voucher-discount")]
public class VoucherDiscountController : ControllerBase
{
    private readonly IVoucherDiscountService _service;
    public VoucherDiscountController(IVoucherDiscountService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? codeSearch = null)
    {
        var result = await _service.GetAllAsync(index, pageSize, codeSearch);
        return Ok(new ResponseModel<PaginatedList<VoucherDiscountResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all voucher discounts successfully!"
        ));
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetAllByCurrentUser()
    {
        var result = await _service.GetAllByCurrentUser();
        return Ok(new ResponseModel<List<VoucherDiscountResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all voucher discounts by current user successfully!"
        ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(new ResponseModel<VoucherDiscountResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get voucher discount successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoucherDiscountRequestDto request)
    {
        await _service.CreateAsync(request);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Create voucher discount successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] VoucherDiscountRequestDto request)
    {
        await _service.UpdateAsync(id, request);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Update voucher discount successfully!"
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
            "Delete voucher discount successfully!"
        ));
    }
}