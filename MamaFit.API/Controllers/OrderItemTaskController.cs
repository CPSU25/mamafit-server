using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
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
        return Ok(new ResponseModel<List<AssignStaffDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            tasks,
            "Get tasks by assigned staff successfully!"
        ));
    }
}