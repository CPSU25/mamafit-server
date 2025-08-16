namespace MamaFit.BusinessObjects.Entity.ChatEntity
{
    public class ChatRoomMember
    {
        public string UserId { get; set; } = string.Empty;
        public string ChatRoomId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
        public ChatRoom? ChatRoom { get; set; }
    }
}
