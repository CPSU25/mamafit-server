using MamaFit.BusinessObjects.DTO.RealtimeDto;
using MamaFit.Services.ExternalService.SignalR;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MamaFit.Services.Hubs;

[Authorize]
public class UnifiedHub : Hub
{
    private readonly IUserConnectionManager _connectionManager;
    private readonly IRealtimeEventService _realtimeEventService;
    private readonly ILogger<UnifiedHub> _logger;

    public UnifiedHub(
        IUserConnectionManager connectionManager,
        IRealtimeEventService realtimeEventService,
        ILogger<UnifiedHub> logger)
    {
        _connectionManager = connectionManager;
        _realtimeEventService = realtimeEventService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("User connected without valid userId. ConnectionId: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
            return;
        }

        try
        {
            // Register user connection
            await _connectionManager.AddUserConnectionAsync(userId, Context.ConnectionId);

            // Add user to appropriate groups based on role
            await AddUserToRoleBasedGroups(userId);

            // Notify others if this is the first connection (user just came online)
            var userConnections = await _connectionManager.GetUserConnectionsAsync(userId);
            if (userConnections.Count == 1)
            {
                await _realtimeEventService.NotifyUserOnlineAsync(userId);
            }

            _logger.LogInformation("User {UserId} connected with ConnectionId {ConnectionId}", userId, Context.ConnectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during connection setup for user {UserId}", userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            try
            {
                await _connectionManager.RemoveUserConnectionAsync(userId, Context.ConnectionId);

                // Check if user is completely offline
                var isStillOnline = await _connectionManager.IsUserOnlineAsync(userId);
                if (!isStillOnline)
                {
                    await _realtimeEventService.NotifyUserOfflineAsync(userId);
                }

                _logger.LogInformation("User {UserId} disconnected. ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during disconnection cleanup for user {UserId}", userId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    // ===== NOTIFICATION HUB METHODS (For Backward Compatibility) =====
    // These methods maintain compatibility with existing NotificationHub clients
    
    /// <summary>
    /// Subscribe to general notifications (backward compatibility)
    /// </summary>
    public async Task SubscribeToNotifications()
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
            return;
        }

        try
        {
            var userGroup = RealtimeGroups.GetUserGroup(userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, userGroup);
            await Groups.AddToGroupAsync(Context.ConnectionId, "notifications_all");
            await Clients.Caller.SendAsync("SubscribedToNotifications", "success");
            
            _logger.LogDebug("User {UserId} subscribed to notifications", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to subscribe user {UserId} to notifications", userId);
            await Clients.Caller.SendAsync("Error", "Failed to subscribe to notifications");
        }
    }

    /// <summary>
    /// Unsubscribe from notifications (backward compatibility)
    /// </summary>
    public async Task UnsubscribeFromNotifications()
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
            return;
        }

        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "notifications_all");
            await Clients.Caller.SendAsync("UnsubscribedFromNotifications", "success");
            
            _logger.LogDebug("User {UserId} unsubscribed from notifications", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unsubscribe user {UserId} from notifications", userId);
            await Clients.Caller.SendAsync("Error", "Failed to unsubscribe from notifications");
        }
    }

    // ===== UNIFIED HUB METHODS =====
    
    // Hub methods that clients can call
    public async Task JoinGroup(string groupName)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
            return;
        }

        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("JoinedGroup", groupName);
            
            _logger.LogDebug("User {UserId} joined group {GroupName}", userId, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user {UserId} to group {GroupName}", userId, groupName);
            await Clients.Caller.SendAsync("Error", $"Failed to join group: {groupName}");
        }
    }

    public async Task LeaveGroup(string groupName)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
            return;
        }

        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("LeftGroup", groupName);
            
            _logger.LogDebug("User {UserId} left group {GroupName}", userId, groupName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove user {UserId} from group {GroupName}", userId, groupName);
            await Clients.Caller.SendAsync("Error", $"Failed to leave group: {groupName}");
        }
    }

    public async Task SubscribeToOrder(string orderId)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
            return;
        }

        var orderGroup = RealtimeGroups.GetOrderGroup(orderId);
        await Groups.AddToGroupAsync(Context.ConnectionId, orderGroup);
        await Clients.Caller.SendAsync("SubscribedToOrder", orderId);
        
        _logger.LogDebug("User {UserId} subscribed to order {OrderId}", userId, orderId);
    }

    public async Task UnsubscribeFromOrder(string orderId)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
            return;
        }

        var orderGroup = RealtimeGroups.GetOrderGroup(orderId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, orderGroup);
        await Clients.Caller.SendAsync("UnsubscribedFromOrder", orderId);
        
        _logger.LogDebug("User {UserId} unsubscribed from order {OrderId}", userId, orderId);
    }

    public async Task GetOnlineUsers()
    {
        try
        {
            var onlineUsers = await _realtimeEventService.GetConnectedUsersAsync();
            await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get online users");
            await Clients.Caller.SendAsync("Error", "Failed to get online users");
        }
    }

    public async Task SendHeartbeat()
    {
        var userId = GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            await _connectionManager.HeartbeatUserConnectionAsync(userId);
            await Clients.Caller.SendAsync("HeartbeatAck", DateTime.UtcNow);
        }
    }

    private async Task AddUserToRoleBasedGroups(string userId)
    {
        try
        {
            // Add user to their personal group
            var userGroup = RealtimeGroups.GetUserGroup(userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, userGroup);

            // Add to role-based groups based on user claims
            var roles = Context.User?.FindAll(ClaimTypes.Role)?.Select(c => c.Value) ?? Enumerable.Empty<string>();
            
            foreach (var role in roles)
            {
                var groupName = role.ToUpperInvariant() switch
                {
                    "ADMIN" => RealtimeGroups.ADMIN_USERS,
                    "MANAGER" => RealtimeGroups.MANAGER_USERS,
                    "STAFF" => RealtimeGroups.STAFF_USERS,
                    "DESIGNER" => RealtimeGroups.DESIGNER_USERS,
                    "CUSTOMER" => RealtimeGroups.CUSTOMER_USERS,
                    _ => null
                };

                if (!string.IsNullOrEmpty(groupName))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                    _logger.LogDebug("Added user {UserId} to role group {GroupName}", userId, groupName);
                }
            }

            // Add to branch-specific group if available
            var branchId = Context.User?.FindFirst("branchId")?.Value;
            if (!string.IsNullOrEmpty(branchId))
            {
                var branchGroup = RealtimeGroups.GetBranchGroup(branchId);
                await Groups.AddToGroupAsync(Context.ConnectionId, branchGroup);
                _logger.LogDebug("Added user {UserId} to branch group {BranchGroup}", userId, branchGroup);
            }

            // Auto-subscribe to notifications for backward compatibility
            await Groups.AddToGroupAsync(Context.ConnectionId, "notifications_all");
            _logger.LogDebug("Added user {UserId} to notifications group", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user {UserId} to role-based groups", userId);
        }
    }

    private string? GetCurrentUserId()
    {
        return Context.User?.FindFirst("userId")?.Value
            ?? Context.User?.FindFirst("sub")?.Value
            ?? Context.User?.FindFirst("nameid")?.Value
            ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}