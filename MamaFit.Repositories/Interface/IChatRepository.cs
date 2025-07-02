using MamaFit.BusinessObjects.Entity.ChatEntity;

namespace MamaFit.Repositories.Interface
{
    public interface IChatRepository
    {
        Task CreateChatMessageAsync(ChatMessage requestDto);
        Task<List<ChatMessage>> GetChatHistoryAsync(string chatRoomId, int page, int pageSize);
        Task<ChatRoom> CreateChatRoomAsync(string userId1, string userId2);
        Task<List<ChatRoom>> GetUserChatRoom(string userId);
        Task<ChatMessage?> GetChatMessageById(string messageId);
        Task<ChatRoom> GetChatRoomById(string chatRoomId);
        Task UpdateMessageAsync(ChatMessage message);
        Task<ChatRoom?> GetRoomBetweenUsersAsync(string userId1, string userId2);
    }
}
