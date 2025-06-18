using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/voucher-batch")]
public class VoucherBatchController : ControllerBase
{
    private readonly IVoucherBatchService _service;
    public VoucherBatchController(IVoucherBatchService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nameSearch = null)
    {
        var voucherBatches = await _service.GetAllVoucherBatchesAsync(index, pageSize, nameSearch);
        return Ok(new ResponseModel<PaginatedList<VoucherBatchResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            voucherBatches,
            "Get all voucher batches successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var voucherBatch = await _service.GetVoucherBatchByIdAsync(id);
        return Ok(new ResponseModel<VoucherBatchResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            voucherBatch,
            "Get voucher batch successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoucherBatchCreateDto requestDto)
    {
        var createdVoucherBatch = await _service.CreateVoucherBatchAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<VoucherBatchResponseDto>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                createdVoucherBatch,
                "Created voucher batch successfully!"
            ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] VoucherBatchUpdateDto requestDto)
    {
        var updatedVoucherBatch = await _service.UpdateVoucherBatchAsync(id, requestDto);
        return Ok(new ResponseModel<VoucherBatchResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            updatedVoucherBatch,
            "Updated voucher batch successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteVoucherBatchAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted voucher batch successfully!"
        ));
    }
}