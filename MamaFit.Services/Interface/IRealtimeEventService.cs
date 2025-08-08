using MamaFit.BusinessObjects.DTO.RealtimeDto;
using MamaFit.Services.Hubs;

namespace MamaFit.Services.Interface;

public interface IRealtimeEventService
{
    // Core event publishing methods
    Task PublishEventAsync(RealtimeEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishEventToUserAsync(string userId, RealtimeEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishEventToUsersAsync(List<string> userIds, RealtimeEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishEventToGroupAsync(string groupName, RealtimeEventDto eventDto, CancellationToken cancellationToken = default);

    // Specific event methods for better type safety
    Task PublishOrderStatusChangedAsync(OrderStatusChangedEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishTaskStatusChangedAsync(TaskStatusChangedEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishPaymentStatusChangedAsync(PaymentStatusChangedEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishNotificationCreatedAsync(NotificationEventDto eventDto, CancellationToken cancellationToken = default);
    Task PublishChatMessageSentAsync(ChatMessageEventDto eventDto, CancellationToken cancellationToken = default);

    // Group management
    Task AddUserToGroupAsync(string userId, string groupName);
    Task RemoveUserFromGroupAsync(string userId, string groupName);
    Task AddConnectionToGroupAsync(string connectionId, string groupName);
    Task RemoveConnectionFromGroupAsync(string connectionId, string groupName);

    // Connection status
    Task NotifyUserOnlineAsync(string userId);
    Task NotifyUserOfflineAsync(string userId);
    
    // Utility methods
    Task<bool> IsRealtimeEnabledForEventType(string eventType);
    Task<List<string>> GetConnectedUsersAsync();
    Task<List<string>> GetUserConnectionsAsync(string userId);
}