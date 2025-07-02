using MamaFit.Services.ExternalService.Redis;

namespace MamaFit.Services.ExternalService.SignalR;

public class UserConnectionManager:IUserConnectionManager
{
    private readonly ICacheService _cacheService;
    private const string UserConnectionsPrefix = "signalr:user-connections:";
    private const string OnlineUsersKey = "signalr:online-users";
    private const string RoomUsersPrefix = "signalr:room-users:";

    public UserConnectionManager(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    // --- User <-> Connection ---
    private string GetUserConnectionKey(string userId) => $"{UserConnectionsPrefix}{userId}";
    private string GetRoomUsersKey(string roomId) => $"{RoomUsersPrefix}{roomId}";

    public async Task AddUserConnectionAsync(string userId, string connectionId)
    {
        var connections = await _cacheService.GetAsync<List<string>>(GetUserConnectionKey(userId)) ?? new List<string>();
        if (!connections.Contains(connectionId))
            connections.Add(connectionId);
        await _cacheService.SetAsync(GetUserConnectionKey(userId), connections, TimeSpan.FromDays(1));

        // Mark user online
        var onlineUsers = await _cacheService.GetAsync<HashSet<string>>(OnlineUsersKey) ?? new HashSet<string>();
        onlineUsers.Add(userId);
        await _cacheService.SetAsync(OnlineUsersKey, onlineUsers, TimeSpan.FromDays(1));
    }

    public async Task RemoveUserConnectionAsync(string userId, string connectionId)
    {
        var connections = await _cacheService.GetAsync<List<string>>(GetUserConnectionKey(userId)) ?? new List<string>();
        connections.Remove(connectionId);
        if (connections.Any())
            await _cacheService.SetAsync(GetUserConnectionKey(userId), connections, TimeSpan.FromDays(1));
        else
        {
            await _cacheService.RemoveAsync(GetUserConnectionKey(userId));
            // Remove khỏi online users
            var onlineUsers = await _cacheService.GetAsync<HashSet<string>>(OnlineUsersKey) ?? new HashSet<string>();
            onlineUsers.Remove(userId);
            await _cacheService.SetAsync(OnlineUsersKey, onlineUsers, TimeSpan.FromDays(1));
        }
    }

    public async Task<List<string>> GetUserConnectionsAsync(string userId)
        => await _cacheService.GetAsync<List<string>>(GetUserConnectionKey(userId)) ?? new List<string>();

    public async Task<List<string>> GetOnlineUsersAsync()
        => (await _cacheService.GetAsync<HashSet<string>>(OnlineUsersKey))?.ToList() ?? new List<string>();

    public async Task<bool> IsUserOnlineAsync(string userId)
    {
        var onlineUsers = await _cacheService.GetAsync<HashSet<string>>(OnlineUsersKey);
        return onlineUsers?.Contains(userId) ?? false;
    }

    // --- Room Management ---
    public async Task AddUserToRoomAsync(string roomId, string userId)
    {
        var users = await _cacheService.GetAsync<HashSet<string>>(GetRoomUsersKey(roomId)) ?? new HashSet<string>();
        users.Add(userId);
        await _cacheService.SetAsync(GetRoomUsersKey(roomId), users, TimeSpan.FromDays(1));
    }

    public async Task RemoveUserFromRoomAsync(string roomId, string userId)
    {
        var users = await _cacheService.GetAsync<HashSet<string>>(GetRoomUsersKey(roomId)) ?? new HashSet<string>();
        if (users.Remove(userId))
        {
            if (users.Count > 0)
                await _cacheService.SetAsync(GetRoomUsersKey(roomId), users, TimeSpan.FromDays(1));
            else
                await _cacheService.RemoveAsync(GetRoomUsersKey(roomId));
        }
    }

    public async Task<List<string>> GetRoomUsersAsync(string roomId)
        => (await _cacheService.GetAsync<HashSet<string>>(GetRoomUsersKey(roomId)))?.ToList() ?? new List<string>();

    public async Task RemoveRoomAsync(string roomId)
        => await _cacheService.RemoveAsync(GetRoomUsersKey(roomId));
}