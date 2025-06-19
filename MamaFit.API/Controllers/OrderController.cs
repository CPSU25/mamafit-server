using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    
    public OrderController(IOrderService service)
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
        var result = await _service.GetAllAsync(index, pageSize, startDate, endDate);
        return Ok(new ResponseModel<PaginatedList<OrderResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all orders successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _service.GetOrderByIdAsync(id);
        return Ok(new ResponseModel<OrderResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get order successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto model)
    {
        var result = await _service.CreateOrderAsync(model);
        return Ok(new ResponseModel<OrderResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Create order successfully!"
        ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] string id, [FromBody] OrderRequestDto model)
    {
        var result = await _service.UpdateOrderAsync(id, model);
        return Ok(new ResponseModel<OrderResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Update order successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder([FromRoute] string id)
    {
        await _service.DeleteOrderAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Delete order successfully!"
        ));
    }
}