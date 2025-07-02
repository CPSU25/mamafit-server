using MamaFit.Services.ExternalService.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MamaFit.Services.Hubs;

public class NotificationHub : Hub
{
    private readonly IUserConnectionManager _connectionManager;
    public NotificationHub(IUserConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            await _connectionManager.AddUserConnectionAsync(userId, Context.ConnectionId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            await _connectionManager.RemoveUserConnectionAsync(userId, Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    private string? GetCurrentUserId()
    {
        var userId = Context.User?.FindFirst("userId")?.Value ??
                     Context.User?.FindFirst("sub")?.Value ??
                     Context.User?.FindFirst("nameid")?.Value ??
                     Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return userId;
    }
}
