using MamaFit.BusinessObjects.Enum;
using System.Text.Json;

namespace MamaFit.BusinessObjects.DTO.RealtimeDto;

public class RealtimeEventDto
{
    public required string EventType { get; set; }
    public required string EntityId { get; set; }
    public required string EntityType { get; set; }
    public required object Data { get; set; }
    public string? UserId { get; set; }
    public string? TargetUserId { get; set; }
    public List<string>? TargetUserIds { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Dictionary<string, object>? Metadata { get; set; }
}

public class OrderStatusChangedEventDto : RealtimeEventDto
{
    public required string OrderId { get; set; }
    public required OrderStatus OldStatus { get; set; }
    public required OrderStatus NewStatus { get; set; }
    public required PaymentStatus PaymentStatus { get; set; }
    public string? OrderCode { get; set; }
}

public class TaskStatusChangedEventDto : RealtimeEventDto
{
    public required string OrderItemTaskId { get; set; }
    public required string OrderItemId { get; set; }
    public required OrderItemTaskStatus OldStatus { get; set; }
    public required OrderItemTaskStatus NewStatus { get; set; }
    public string? TaskName { get; set; }
    public string? OrderCode { get; set; }
}

public class PaymentStatusChangedEventDto : RealtimeEventDto
{
    public required string TransactionId { get; set; }
    public required string OrderId { get; set; }
    public required decimal Amount { get; set; }
    public required PaymentStatus PaymentStatus { get; set; }
    public string? Gateway { get; set; }
    public string? OrderCode { get; set; }
}

public class NotificationEventDto : RealtimeEventDto
{
    public required string NotificationId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required NotificationType NotificationType { get; set; }
    public string? ActionUrl { get; set; }
}

public class ChatMessageEventDto : RealtimeEventDto
{
    public required string MessageId { get; set; }
    public required string ChatRoomId { get; set; }
    public required string SenderId { get; set; }
    public required string Message { get; set; }
    public required MessageType MessageType { get; set; }
    public string? SenderName { get; set; }
}