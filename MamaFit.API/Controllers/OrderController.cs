using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("by-token")]
    public async Task<IActionResult> GetByAccessToken(
        [FromHeader(Name = "Authorization")] string accessToken,
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] OrderStatus? status = null)
    {
        var result = await _service.GetByTokenAsync(accessToken, index, pageSize, search, status);
        return Ok(new ResponseModel<PaginatedList<OrderResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get orders by access token successfully!"
        ));
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

    [HttpPost]
    [Route("ready-to-buy")]
    public async Task<IActionResult> CreateOrderReadyToBuy([FromBody] OrderReadyToBuyRequestDto model)
    {
        var response = await _service.CreateReadyToBuyOrderAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            response,
            "Create order ready to buy successfully!"
        ));
    }

    [HttpPost]
    [Route("design-request")]
    public async Task<IActionResult> CreateDesignRequest([FromBody] OrderDesignRequestDto model)
    {
        var response = await _service.CreateDesignRequestOrderAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            response,
            "Create design request successfully!"
        ));
    }

    [HttpPost]
    [Route("preset")]
    public async Task<IActionResult> CreatePresetOrder([FromBody] OrderPresetCreateRequestDto model)
    {
        var response = await _service.CreatePresetOrderAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            response,
            "Create preset order successfully!"
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

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(
        [FromRoute] string id,
        [FromQuery] OrderStatus orderStatus,
        [FromQuery] PaymentStatus paymentStatus)
    {
        await _service.UpdateOrderStatusAsync(id, orderStatus, paymentStatus);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Update order status successfully!"
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