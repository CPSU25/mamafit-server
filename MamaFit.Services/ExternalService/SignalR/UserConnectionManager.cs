using MamaFit.Services.ExternalService.Redis;

namespace MamaFit.Services.ExternalService.SignalR;

public class UserConnectionManager : IUserConnectionManager
{
    private readonly ICacheService _cacheService;
    private const string UserConnectionsPrefix = "signalr:user-connections:";
    private const string RoomUsersPrefix = "signalr:room-users:";

    private static readonly TimeSpan ConnectionTtl = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan RoomTtl = TimeSpan.FromDays(1);

    public UserConnectionManager(ICacheService cacheService)
    {
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    private string GetUserConnectionKey(string userId) => $"{UserConnectionsPrefix}{userId}";
    private string GetRoomUsersKey(string roomId) => $"{RoomUsersPrefix}{roomId}";

    public async Task AddUserConnectionAsync(string userId, string connectionId)
    {
        await _cacheService.SetAddAsync(GetUserConnectionKey(userId), connectionId, ConnectionTtl);
    }

    public async Task RemoveUserConnectionAsync(string userId, string connectionId)
    {
        await _cacheService.SetRemoveAsync(GetUserConnectionKey(userId), connectionId);
        var remaining = await _cacheService.SetLengthAsync(GetUserConnectionKey(userId));
        if (remaining == 0)
            await _cacheService.RemoveKeyAsync(GetUserConnectionKey(userId));
    }

    public async Task<List<string>> GetUserConnectionsAsync(string userId)
    {
        return await _cacheService.SetMembersAsync(GetUserConnectionKey(userId));
    }

    public async Task<bool> IsUserOnlineAsync(string userId)
    {
        return await _cacheService.KeyExistsAsync(GetUserConnectionKey(userId));
    }

    public async Task<List<string>> GetOnlineUsersAsync()
    {
        // Nếu bạn muốn scan hết key user-connections, hãy bổ sung thêm hàm ScanKeys ở CacheService nếu cần.
        throw new NotImplementedException("Scan online users by key pattern should be implemented as needed.");
    }

    // --- Room Management ---
    public async Task AddUserToRoomAsync(string roomId, string userId)
    {
        await _cacheService.SetAddAsync(GetRoomUsersKey(roomId), userId, RoomTtl);
    }

    public async Task RemoveUserFromRoomAsync(string roomId, string userId)
    {
        await _cacheService.SetRemoveAsync(GetRoomUsersKey(roomId), userId);
        var count = await _cacheService.SetLengthAsync(GetRoomUsersKey(roomId));
        if (count == 0)
            await _cacheService.RemoveKeyAsync(GetRoomUsersKey(roomId));
    }

    public async Task<List<string>> GetRoomUsersAsync(string roomId)
    {
        return await _cacheService.SetMembersAsync(GetRoomUsersKey(roomId));
    }

    public async Task RemoveRoomAsync(string roomId)
    {
        await _cacheService.RemoveKeyAsync(GetRoomUsersKey(roomId));
    }

    public async Task HeartbeatUserConnectionAsync(string userId)
    {
        await _cacheService.KeyExpireAsync(GetUserConnectionKey(userId), ConnectionTtl);
    }
}