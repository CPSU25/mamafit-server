using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity.ChatEntity
{
    public class ChatMessage : BaseEntity
    {
        public string? Message { get; set; } 
        public string? SenderId { get; set; } 
        public string? ChatRoomId { get; set; }
        public MessageType Type { get; set; } = MessageType.Text;
        public bool IsRead { get; set; } = false;
        public ApplicationUser? Sender { get; set; }
        public ChatRoom? ChatRoom { get; set; } = null!;

    }
}
