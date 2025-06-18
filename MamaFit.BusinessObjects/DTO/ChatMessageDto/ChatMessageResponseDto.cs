using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.ChatMessageDto
{
    public class ChatMessageResponseDto
    {
        public string? Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderAvatar { get; set; } = string.Empty;
        public DateTime MessageTimestamp { get; set; }
        public string ChatRoomId { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;
    }
}
