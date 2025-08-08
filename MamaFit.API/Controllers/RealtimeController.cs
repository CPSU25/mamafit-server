using MamaFit.BusinessObjects.DTO.RealtimeDto;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealtimeController : ControllerBase
{
    private readonly IRealtimeEventService _realtimeEventService;

    public RealtimeController(IRealtimeEventService realtimeEventService)
    {
        _realtimeEventService = realtimeEventService;
    }

    /// <summary>
    /// Get all connected users (for admin/debugging purposes)
    /// </summary>
    [HttpGet("connected-users")]
    [Authorize]
    public async Task<IActionResult> GetConnectedUsers()
    {
        var connectedUsers = await _realtimeEventService.GetConnectedUsersAsync();
        return Ok(new { ConnectedUsers = connectedUsers, Count = connectedUsers.Count });
    }

    /// <summary>
    /// Test broadcast a system announcement to all users
    /// </summary>
    [HttpPost("test-announcement")]
    [Authorize]
    public async Task<IActionResult> TestAnnouncement([FromBody] string message)
    {
        var eventDto = new RealtimeEventDto
        {
            EventType = RealtimeEventTypes.SYSTEM_ANNOUNCEMENT,
            EntityId = Guid.NewGuid().ToString(),
            EntityType = "System",
            Data = new { Message = message, Sender = "Admin" },
            Timestamp = DateTime.UtcNow
        };

        await _realtimeEventService.PublishEventAsync(eventDto);
        return Ok(new { Success = true, Message = "Announcement sent to all connected users" });
    }

    /// <summary>
    /// Test order status change event for a specific user
    /// </summary>
    [HttpPost("test-order-status")]
    [Authorize]
    public async Task<IActionResult> TestOrderStatusChange([FromBody] TestOrderStatusRequest request)
    {
        var eventDto = new OrderStatusChangedEventDto
        {
            EventType = RealtimeEventTypes.ORDER_STATUS_CHANGED,
            EntityId = request.OrderId,
            EntityType = RealtimeEntityTypes.ORDER,
            Data = new
            {
                OrderId = request.OrderId,
                OrderCode = request.OrderCode,
                OldStatus = request.OldStatus,
                NewStatus = request.NewStatus,
                UserId = request.UserId
            },
            TargetUserId = request.UserId,
            OrderId = request.OrderId,
            OldStatus = Enum.Parse<BusinessObjects.Enum.OrderStatus>(request.OldStatus),
            NewStatus = Enum.Parse<BusinessObjects.Enum.OrderStatus>(request.NewStatus),
            PaymentStatus = Enum.Parse<BusinessObjects.Enum.PaymentStatus>(request.PaymentStatus),
            OrderCode = request.OrderCode,
            Metadata = new Dictionary<string, object>
            {
                { "orderId", request.OrderId },
                { "userId", request.UserId },
                { "orderCode", request.OrderCode }
            }
        };

        await _realtimeEventService.PublishOrderStatusChangedAsync(eventDto);
        return Ok(new { Success = true, Message = $"Order status change event sent to user {request.UserId}" });
    }

    /// <summary>
    /// Test task status change event
    /// </summary>
    [HttpPost("test-task-status")]
    [Authorize]
    public async Task<IActionResult> TestTaskStatusChange([FromBody] TestTaskStatusRequest request)
    {
        var eventDto = new TaskStatusChangedEventDto
        {
            EventType = RealtimeEventTypes.TASK_STATUS_CHANGED,
            EntityId = request.TaskId,
            EntityType = RealtimeEntityTypes.ORDER_ITEM_TASK,
            Data = new
            {
                TaskId = request.TaskId,
                TaskName = request.TaskName,
                OldStatus = request.OldStatus,
                NewStatus = request.NewStatus,
                OrderId = request.OrderId,
                OrderCode = request.OrderCode,
                AssignedStaffId = request.AssignedStaffId,
                CustomerId = request.CustomerId
            },
            TargetUserId = request.CustomerId,
            TargetUserIds = new List<string> { request.CustomerId, request.AssignedStaffId },
            OrderItemTaskId = request.TaskId,
            OrderItemId = request.OrderItemId,
            OldStatus = Enum.Parse<BusinessObjects.Enum.OrderItemTaskStatus>(request.OldStatus),
            NewStatus = Enum.Parse<BusinessObjects.Enum.OrderItemTaskStatus>(request.NewStatus),
            TaskName = request.TaskName,
            OrderCode = request.OrderCode,
            Metadata = new Dictionary<string, object>
            {
                { "orderId", request.OrderId },
                { "orderCode", request.OrderCode },
                { "customerId", request.CustomerId },
                { "assignedStaffId", request.AssignedStaffId }
            }
        };

        await _realtimeEventService.PublishTaskStatusChangedAsync(eventDto);
        return Ok(new { Success = true, Message = $"Task status change event sent" });
    }
}

public class TestOrderStatusRequest
{
    public required string OrderId { get; set; }
    public required string OrderCode { get; set; }
    public required string UserId { get; set; }
    public required string OldStatus { get; set; }
    public required string NewStatus { get; set; }
    public required string PaymentStatus { get; set; }
}

public class TestTaskStatusRequest
{
    public required string TaskId { get; set; }
    public required string OrderItemId { get; set; }
    public required string OrderId { get; set; }
    public required string OrderCode { get; set; }
    public required string TaskName { get; set; }
    public required string OldStatus { get; set; }
    public required string NewStatus { get; set; }
    public required string AssignedStaffId { get; set; }
    public required string CustomerId { get; set; }
}