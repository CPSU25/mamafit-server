using MamaFit.BusinessObjects.DTO.RealtimeDto;
using MamaFit.Services.ExternalService.SignalR;
using MamaFit.Services.Hubs;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MamaFit.Services.Service;

public class RealtimeEventService : IRealtimeEventService
{
    private readonly IHubContext<UnifiedHub> _hubContext;
    private readonly IUserConnectionManager _connectionManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RealtimeEventService> _logger;
    private readonly RealtimeConfigDto _config;

    // In a production environment, you need to register this as a service
    // This should depend on IHubContext<UnifiedHub> but we'll fix the reference issue
    public RealtimeEventService(
        IHubContext<UnifiedHub> hubContext,
        IUserConnectionManager connectionManager,
        IConfiguration configuration,
        ILogger<RealtimeEventService> logger)
    {
        _hubContext = hubContext;
        _connectionManager = connectionManager;
        _configuration = configuration;
        _logger = logger;
        _config = _configuration.GetSection("RealtimeConfig").Get<RealtimeConfigDto>() ?? new RealtimeConfigDto();
    }

    public async Task PublishEventAsync(RealtimeEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!await IsRealtimeEnabledForEventType(eventDto.EventType))
        {
            _logger.LogDebug("Realtime disabled for event type: {EventType}", eventDto.EventType);
            return;
        }

        try
        {
            await _hubContext.Clients.All.SendAsync("RealtimeEvent", eventDto, cancellationToken);
            _logger.LogInformation("Published event {EventType} for entity {EntityId}", 
                eventDto.EventType, eventDto.EntityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType} for entity {EntityId}", 
                eventDto.EventType, eventDto.EntityId);
            
            if (_config.LogFailedEvents)
            {
                // Log failed event for later retry or analysis
                await LogFailedEventAsync(eventDto, ex);
            }
        }
    }

    public async Task PublishEventToUserAsync(string userId, RealtimeEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!await IsRealtimeEnabledForEventType(eventDto.EventType))
            return;

        try
        {
            var connections = await _connectionManager.GetUserConnectionsAsync(userId);
            if (connections.Any())
            {
                foreach (var connectionId in connections)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("RealtimeEvent", eventDto, cancellationToken);
                }
                
                _logger.LogInformation("Published event {EventType} to user {UserId} with {ConnectionCount} connections", 
                    eventDto.EventType, userId, connections.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType} to user {UserId}", 
                eventDto.EventType, userId);
        }
    }

    public async Task PublishEventToUsersAsync(List<string> userIds, RealtimeEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!await IsRealtimeEnabledForEventType(eventDto.EventType))
            return;

        var tasks = userIds.Select(userId => PublishEventToUserAsync(userId, eventDto, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public async Task PublishEventToGroupAsync(string groupName, RealtimeEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!await IsRealtimeEnabledForEventType(eventDto.EventType))
            return;

        try
        {
            await _hubContext.Clients.Group(groupName).SendAsync("RealtimeEvent", eventDto, cancellationToken);
            _logger.LogInformation("Published event {EventType} to group {GroupName}", 
                eventDto.EventType, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType} to group {GroupName}", 
                eventDto.EventType, groupName);
        }
    }

    public async Task PublishOrderStatusChangedAsync(OrderStatusChangedEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!_config.EnableOrderStatusUpdates)
            return;

        // Publish to specific user
        if (!string.IsNullOrEmpty(eventDto.TargetUserId))
        {
            await PublishEventToUserAsync(eventDto.TargetUserId, eventDto, cancellationToken);
        }

        // Publish to order group (staff working on this order)
        var orderGroup = RealtimeGroups.GetOrderGroup(eventDto.OrderId);
        await PublishEventToGroupAsync(orderGroup, eventDto, cancellationToken);

        // Publish to admin/manager groups
        await PublishEventToGroupAsync(RealtimeGroups.ADMIN_USERS, eventDto, cancellationToken);
        await PublishEventToGroupAsync(RealtimeGroups.MANAGER_USERS, eventDto, cancellationToken);
    }

    public async Task PublishTaskStatusChangedAsync(TaskStatusChangedEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!_config.EnableTaskStatusUpdates)
            return;

        // Publish to specific user or users
        if (!string.IsNullOrEmpty(eventDto.TargetUserId))
        {
            await PublishEventToUserAsync(eventDto.TargetUserId, eventDto, cancellationToken);
        }

        if (eventDto.TargetUserIds?.Any() == true)
        {
            await PublishEventToUsersAsync(eventDto.TargetUserIds, eventDto, cancellationToken);
        }

        // Publish to order group
        var orderGroup = RealtimeGroups.GetOrderGroup(eventDto.Metadata?["orderId"]?.ToString() ?? "");
        if (!string.IsNullOrEmpty(orderGroup))
        {
            await PublishEventToGroupAsync(orderGroup, eventDto, cancellationToken);
        }

        // Publish to staff groups
        await PublishEventToGroupAsync(RealtimeGroups.STAFF_USERS, eventDto, cancellationToken);
        await PublishEventToGroupAsync(RealtimeGroups.DESIGNER_USERS, eventDto, cancellationToken);
    }

    public async Task PublishPaymentStatusChangedAsync(PaymentStatusChangedEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!_config.EnablePaymentStatusUpdates)
            return;

        // Publish to specific user
        if (!string.IsNullOrEmpty(eventDto.TargetUserId))
        {
            await PublishEventToUserAsync(eventDto.TargetUserId, eventDto, cancellationToken);
        }

        // Publish to admin groups
        await PublishEventToGroupAsync(RealtimeGroups.ADMIN_USERS, eventDto, cancellationToken);
        await PublishEventToGroupAsync(RealtimeGroups.MANAGER_USERS, eventDto, cancellationToken);
    }

    public async Task PublishNotificationCreatedAsync(NotificationEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!_config.EnableNotificationUpdates)
            return;

        // Publish to specific user
        if (!string.IsNullOrEmpty(eventDto.TargetUserId))
        {
            await PublishEventToUserAsync(eventDto.TargetUserId, eventDto, cancellationToken);
        }
    }

    public async Task PublishChatMessageSentAsync(ChatMessageEventDto eventDto, CancellationToken cancellationToken = default)
    {
        if (!_config.EnableChatUpdates)
            return;

        // Publish to chat room group
        var chatRoomGroup = RealtimeGroups.GetChatRoomGroup(eventDto.ChatRoomId);
        await PublishEventToGroupAsync(chatRoomGroup, eventDto, cancellationToken);
    }

    public async Task AddUserToGroupAsync(string userId, string groupName)
    {
        try
        {
            var connections = await _connectionManager.GetUserConnectionsAsync(userId);
            foreach (var connectionId in connections)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
            }
            
            _logger.LogDebug("Added user {UserId} to group {GroupName}", userId, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user {UserId} to group {GroupName}", userId, groupName);
        }
    }

    public async Task RemoveUserFromGroupAsync(string userId, string groupName)
    {
        try
        {
            var connections = await _connectionManager.GetUserConnectionsAsync(userId);
            foreach (var connectionId in connections)
            {
                await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
            }
            
            _logger.LogDebug("Removed user {UserId} from group {GroupName}", userId, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove user {UserId} from group {GroupName}", userId, groupName);
        }
    }

    public async Task AddConnectionToGroupAsync(string connectionId, string groupName)
    {
        try
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
            _logger.LogDebug("Added connection {ConnectionId} to group {GroupName}", connectionId, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add connection {ConnectionId} to group {GroupName}", connectionId, groupName);
        }
    }

    public async Task RemoveConnectionFromGroupAsync(string connectionId, string groupName)
    {
        try
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
            _logger.LogDebug("Removed connection {ConnectionId} from group {GroupName}", connectionId, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove connection {ConnectionId} from group {GroupName}", connectionId, groupName);
        }
    }

    public async Task NotifyUserOnlineAsync(string userId)
    {
        var eventDto = new RealtimeEventDto
        {
            EventType = "user.online",
            EntityId = userId,
            EntityType = RealtimeEntityTypes.USER,
            Data = new { UserId = userId, Status = "online" },
            Timestamp = DateTime.UtcNow
        };

        await PublishEventAsync(eventDto);
    }

    public async Task NotifyUserOfflineAsync(string userId)
    {
        var eventDto = new RealtimeEventDto
        {
            EventType = "user.offline",
            EntityId = userId,
            EntityType = RealtimeEntityTypes.USER,
            Data = new { UserId = userId, Status = "offline" },
            Timestamp = DateTime.UtcNow
        };

        await PublishEventAsync(eventDto);
    }

    public Task<bool> IsRealtimeEnabledForEventType(string eventType)
    {
        return Task.FromResult(eventType switch
        {
            var t when t.StartsWith("order") => _config.EnableOrderStatusUpdates,
            var t when t.StartsWith("task") => _config.EnableTaskStatusUpdates,
            var t when t.StartsWith("payment") => _config.EnablePaymentStatusUpdates,
            var t when t.StartsWith("notification") => _config.EnableNotificationUpdates,
            var t when t.StartsWith("chat") => _config.EnableChatUpdates,
            var t when t.StartsWith("warranty") => _config.EnableWarrantyStatusUpdates,
            _ => true
        });
    }

    public async Task<List<string>> GetConnectedUsersAsync()
    {
        return await _connectionManager.GetOnlineUsersAsync();
    }

    public async Task<List<string>> GetUserConnectionsAsync(string userId)
    {
        return await _connectionManager.GetUserConnectionsAsync(userId);
    }

    private Task LogFailedEventAsync(RealtimeEventDto eventDto, Exception exception)
    {
        // In a production environment, you might want to store this in a database or external logging service
        var failedEventLog = new
        {
            Event = eventDto,
            Exception = exception.ToString(),
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(failedEventLog, new JsonSerializerOptions { WriteIndented = true });
        _logger.LogError("Failed Event Log: {FailedEventJson}", json);
        
        return Task.CompletedTask;
    }
}