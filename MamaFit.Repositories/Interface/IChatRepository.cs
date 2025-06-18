using MamaFit.BusinessObjects.Entity.ChatEntity;

namespace MamaFit.Repositories.Interface
{
    public interface IChatRepository
    {
        Task CreateChatMessageAsync(ChatMessage requestDto);
        Task<List<ChatMessage>> GetChatHistoryAsync(string chatRoomId, int index, int pageSize);
        Task CreateChatRoomAsync(string userId1, string userId2);
        Task<List<ChatRoom>> GetUserChatRoom(string userId);
        Task<ChatMessage> GetChatMessageById(string messageId);
        Task<ChatRoom> GetChatRoomById(string chatRoomId);
        Task UpdateMessageAsync(ChatMessage message);
    }
}
