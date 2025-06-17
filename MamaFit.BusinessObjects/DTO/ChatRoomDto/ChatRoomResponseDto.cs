using MamaFit.BusinessObjects.DTO.ChatRoomMemberDto;

namespace MamaFit.BusinessObjects.DTO.ChatRoomDto
{
    public class ChatRoomResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastTimestamp { get; set; }
        public string LastUserId { get; set; } = string.Empty;
        public string LastUserName { get; set; } = string.Empty;
        public List<ChatRoomMemberResponseDto> Members { get; set; } = [];
    }
}
