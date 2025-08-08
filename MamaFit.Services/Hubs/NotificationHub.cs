using MamaFit.Services.ExternalService.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MamaFit.Services.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _connectionManager;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(
            IUserConnectionManager connectionManager,
            ILogger<NotificationHub> logger)
        {
            _connectionManager = connectionManager;
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

                // Add user to their personal notification group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"notifications_user_{userId}");

                // Add to role-based notification groups
                await AddUserToRoleBasedNotificationGroups(userId);

                // Auto-subscribe to general notifications
                await Groups.AddToGroupAsync(Context.ConnectionId, "notifications_all");

                _logger.LogInformation("User {UserId} connected to NotificationHub with ConnectionId {ConnectionId}", userId, Context.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during notification hub connection setup for user {UserId}", userId);
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
                    _logger.LogInformation("User {UserId} disconnected from NotificationHub. ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during notification hub disconnection cleanup for user {UserId}", userId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Subscribe to general notifications
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
                await Groups.AddToGroupAsync(Context.ConnectionId, "notifications_all");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"notifications_user_{userId}");
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
        /// Unsubscribe from general notifications
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

        /// <summary>
        /// Subscribe to order-specific notifications
        /// </summary>
        public async Task SubscribeToOrder(string orderId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"notifications_order_{orderId}");
                await Clients.Caller.SendAsync("SubscribedToOrder", orderId);
                
                _logger.LogDebug("User {UserId} subscribed to order notifications {OrderId}", userId, orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe user {UserId} to order {OrderId} notifications", userId, orderId);
                await Clients.Caller.SendAsync("Error", $"Failed to subscribe to order notifications: {orderId}");
            }
        }

        /// <summary>
        /// Unsubscribe from order-specific notifications
        /// </summary>
        public async Task UnsubscribeFromOrder(string orderId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"notifications_order_{orderId}");
                await Clients.Caller.SendAsync("UnsubscribedFromOrder", orderId);
                
                _logger.LogDebug("User {UserId} unsubscribed from order notifications {OrderId}", userId, orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to unsubscribe user {UserId} from order {OrderId} notifications", userId, orderId);
                await Clients.Caller.SendAsync("Error", $"Failed to unsubscribe from order notifications: {orderId}");
            }
        }

        /// <summary>
        /// Subscribe to appointment notifications
        /// </summary>
        public async Task SubscribeToAppointments()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "notifications_appointments");
                await Clients.Caller.SendAsync("SubscribedToAppointments", "success");
                
                _logger.LogDebug("User {UserId} subscribed to appointment notifications", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe user {UserId} to appointment notifications", userId);
                await Clients.Caller.SendAsync("Error", "Failed to subscribe to appointment notifications");
            }
        }

        /// <summary>
        /// Subscribe to payment notifications
        /// </summary>
        public async Task SubscribeToPayments()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "notifications_payments");
                await Clients.Caller.SendAsync("SubscribedToPayments", "success");
                
                _logger.LogDebug("User {UserId} subscribed to payment notifications", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe user {UserId} to payment notifications", userId);
                await Clients.Caller.SendAsync("Error", "Failed to subscribe to payment notifications");
            }
        }

        /// <summary>
        /// Mark notification as read
        /// </summary>
        public async Task MarkNotificationAsRead(string notificationId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            try
            {
                // Logic to mark notification as read in database
                // This would be implemented in a service
                await Clients.Caller.SendAsync("NotificationMarkedAsRead", notificationId);
                
                _logger.LogDebug("User {UserId} marked notification {NotificationId} as read", userId, notificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark notification {NotificationId} as read for user {UserId}", notificationId, userId);
                await Clients.Caller.SendAsync("Error", "Failed to mark notification as read");
            }
        }

        /// <summary>
        /// Get unread notification count
        /// </summary>
        public async Task GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            try
            {
                // Logic to get unread count from database
                // This would be implemented in a service
                var unreadCount = 0; // Placeholder
                await Clients.Caller.SendAsync("UnreadCount", unreadCount);
                
                _logger.LogDebug("Retrieved unread count {Count} for user {UserId}", unreadCount, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get unread count for user {UserId}", userId);
                await Clients.Caller.SendAsync("Error", "Failed to get unread count");
            }
        }

        private async Task AddUserToRoleBasedNotificationGroups(string userId)
        {
            try
            {
                // Add to role-based notification groups based on user claims
                var roles = Context.User?.FindAll(ClaimTypes.Role)?.Select(c => c.Value) ?? Enumerable.Empty<string>();
                
                foreach (var role in roles)
                {
                    var groupName = role.ToUpperInvariant() switch
                    {
                        "ADMIN" => "notifications_admin",
                        "MANAGER" => "notifications_manager",
                        "STAFF" => "notifications_staff",
                        "DESIGNER" => "notifications_designer",
                        "CUSTOMER" => "notifications_customer",
                        _ => null
                    };

                    if (!string.IsNullOrEmpty(groupName))
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                        _logger.LogDebug("Added user {UserId} to notification group {GroupName}", userId, groupName);
                    }
                }

                // Add to branch-specific notification group if available
                var branchId = Context.User?.FindFirst("branchId")?.Value;
                if (!string.IsNullOrEmpty(branchId))
                {
                    var branchGroup = $"notifications_branch_{branchId}";
                    await Groups.AddToGroupAsync(Context.ConnectionId, branchGroup);
                    _logger.LogDebug("Added user {UserId} to branch notification group {BranchGroup}", userId, branchGroup);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add user {UserId} to role-based notification groups", userId);
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
}