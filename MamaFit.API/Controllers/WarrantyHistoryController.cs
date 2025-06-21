using MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/warranty-history")]
public class WarrantyHistoryController : ControllerBase
{
    private readonly IWarrantyHistoryService _historyService;
    public WarrantyHistoryController(IWarrantyHistoryService historyService)
    {
        _historyService = historyService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _historyService.GetAllAsync(index, pageSize, startDate, endDate);
        return Ok(new ResponseModel<PaginatedList<WarrantyHistoryResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all warranty histories successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _historyService.GetByIdAsync(id);
        return Ok(new ResponseModel<WarrantyHistoryResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get warranty history successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WarrantyHistoryRequestDto request)
    {
        var result = await _historyService.CreateAsync(request);
        return Ok(new ResponseModel<WarrantyHistoryResponseDto>(
            StatusCodes.Status201Created,
            ApiCodes.SUCCESS,
            result,
            "Create warranty history successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] WarrantyHistoryRequestDto request)
    {
        var result = await _historyService.UpdateAsync(id, request);
        return Ok(new ResponseModel<WarrantyHistoryResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Update warranty history successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _historyService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Delete warranty history successfully!"
        ));
    }
}