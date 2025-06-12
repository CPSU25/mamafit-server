using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity.ChatEntity
{
    public class ChatMessage : BaseEntity
    {
        public string Message { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string ChatRoomId { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;

        //

        public ApplicationUser Sender { get; set; } = null!;
        public ChatRoom ChatRoom { get; set; } = null!;

    }
}
