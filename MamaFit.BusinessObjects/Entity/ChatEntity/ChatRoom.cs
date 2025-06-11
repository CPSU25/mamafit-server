using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity.ChatEntity
{
    public class ChatRoom : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    }
}
