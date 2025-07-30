using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/order-item-tasks")]
public class OrderItemTaskController : ControllerBase
{
    private readonly IOrderItemTaskService _orderItemTaskService;
    
    public OrderItemTaskController(IOrderItemTaskService orderItemTaskService)
    {
        _orderItemTaskService = orderItemTaskService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetTasksByAssignedStaffAsync(
        [FromHeader(Name = "Authorization")] string accessToken)
    {
        var tasks = await _orderItemTaskService.GetTasksByAssignedStaffAsync(accessToken);
        return Ok(new ResponseModel<List<OrderItemTaskGetByTokenResponse>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            tasks,
            "Get tasks by assigned staff successfully!"
        ));
    }

    [HttpPut("{dressTaskId}/{orderItemId}")]
    [Authorize]
    public async Task<IActionResult> UpdateStatusAsync(
        [FromRoute] string dressTaskId,
        [FromRoute] string orderItemId,
        [FromForm] OrderItemTaskStatus status)
    { 
        await _orderItemTaskService.UpdateStatusAsync(dressTaskId, orderItemId, status);
        return Ok(new ResponseModel(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            "Update order item task status successfully!"
        ));
    }
}