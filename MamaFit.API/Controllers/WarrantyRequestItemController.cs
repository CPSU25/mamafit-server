using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/warranty-request-item")]
public class WarrantyRequestItemController : ControllerBase
{
    private readonly IWarrantyRequestItemService _warrantyRequestItemService;

    public WarrantyRequestItemController(IWarrantyRequestItemService warrantyRequestItemService)
    {
        _warrantyRequestItemService = warrantyRequestItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var warrantyRequestItems = await _warrantyRequestItemService.GetAllAsync(index, pageSize, search);
        return Ok(new ResponseModel<PaginatedList<WarrantyRequestItemGetAllDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            warrantyRequestItems,
            "Get all warranty request items successfully!")
        );
    }

    [HttpGet("order-item/{orderItemId}")]
    public async Task<IActionResult> GetByOrderItemId(string orderItemId)
    {
        var result = await _warrantyRequestItemService.GetDetailsByOrderItemIdAsync(orderItemId);
        return Ok(new ResponseModel<WarrantyRequestItemDetailDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all warranty request items successfully!")
        );
    }

    [HttpGet("order-item-list/{orderItemId}")]
    public async Task<IActionResult> GetAllRelatedByOrderItemId(string orderItemId)
    {
        var result = await _warrantyRequestItemService.GetAllDetailsByOrderItemIdAsync(orderItemId);
        return Ok(new ResponseModel<List<WarrantyRequestItemDetailListDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all related warranty request items successfully!")
        );
    }
}