using MamaFit.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.DTO.ChatMessageDto
{
    public class ChatMessageCreateDto
    {
        public string Message { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string ChatRoomId { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;
    }
}
