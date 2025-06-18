
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.BusinessObjects.DTO.ChatRoomDto;

namespace MamaFit.Services.Interface
{
    public interface IChatService
    {
        Task<ChatMessageResponseDto> CreateChatMessageAsync(ChatMessageCreateDto requestDto);
        Task<List<ChatMessageResponseDto>> GetChatHistoryAsync(string chatRoomId, int index, int pageSize);
        Task CreateChatRoomAsync(string userId1, string userId2);
        Task<List<ChatRoomResponseDto>> GetUserChatRoom(string userId);
        Task<ChatMessageResponseDto> GetChatMessageById(string messageId);
        Task<ChatRoomResponseDto> GetChatRoomById(string chatRoomId);
        Task MarkMessageAsReadAsync(string messageId, string userId, string chatRoomId);
    }
}
