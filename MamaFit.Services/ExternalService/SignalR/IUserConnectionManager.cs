namespace MamaFit.Services.ExternalService.SignalR;

public interface IUserConnectionManager
{
    // Connection
    Task AddUserConnectionAsync(string userId, string connectionId);
    Task RemoveUserConnectionAsync(string userId, string connectionId);
    Task<List<string>> GetUserConnectionsAsync(string userId);

    // Online users (toàn hệ thống)
    Task<List<string>> GetOnlineUsersAsync();
    Task<bool> IsUserOnlineAsync(string userId);

    // Room
    Task AddUserToRoomAsync(string roomId, string userId);
    Task RemoveUserFromRoomAsync(string roomId, string userId);
    Task<List<string>> GetRoomUsersAsync(string roomId);

    // Optional: Remove all user from room (khi giải tán phòng)
    Task RemoveRoomAsync(string roomId);
}