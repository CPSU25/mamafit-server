using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/order-items")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _service;
    
    public OrderItemController(IOrderItemService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _service.GetAllOrderItemsAsync(index, pageSize, startDate, endDate);
        return Ok(new ResponseModel<PaginatedList<OrderItemResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all order items successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _service.GetOrderItemByIdAsync(id);
        return Ok(new ResponseModel<OrderItemResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get order item successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderItemRequestDto model)
    {
        var result = await _service.CreateOrderItemAsync(model);
        return Ok(new ResponseModel<OrderItemResponseDto>(
            StatusCodes.Status201Created,
            ApiCodes.SUCCESS,
            result,
            "Create order item successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] OrderItemRequestDto model)
    {
        var result = await _service.UpdateOrderItemAsync(id, model);
        return Ok(new ResponseModel<OrderItemResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Update order item successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteOrderItemAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Delete order item successfully!"
        ));
    }
}