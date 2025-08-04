using MamaFit.BusinessObjects.Enum;
using System.Text.Json;

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
        public bool IsRead { get; set; }
        public string ChatRoomId { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Text;
        
        /// <summary>
        /// Automatically parse JSON message if Type is JSON. Returns parsed object with messageContent, OrderId, DesignRequestId
        /// </summary>
        public object? ParsedData
        {
            get
            {
                if (Type == MessageType.Design_Request && !string.IsNullOrEmpty(Message))
                {
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        return JsonSerializer.Deserialize<JsonElement>(Message, options);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        
        /// <summary>
        /// Get messageContent từ JSON parsed data
        /// </summary>
        public string? MessageContent
        {
            get
            {
                if (ParsedData is JsonElement element && element.TryGetProperty("messageContent", out var content))
                {
                    return content.GetString();
                }
                return Type == MessageType.Design_Request ? null : Message;
            }
        }
        
        /// <summary>
        /// Get OrderId từ JSON parsed data
        /// </summary>
        public string? OrderId
        {
            get
            {
                if (ParsedData is JsonElement element && element.TryGetProperty("orderId", out var orderId))
                {
                    return orderId.GetString();
                }
                return null;
            }
        }
        
        /// <summary>
        /// Get DesignRequestId từ JSON parsed data
        /// </summary>
        public string? DesignRequestId
        {
            get
            {
                if (ParsedData is JsonElement element && element.TryGetProperty("designRequestId", out var designRequestId))
                {
                    return designRequestId.GetString();
                }
                return null;
            }
        }
        
        /// <summary>
        /// Kiểm tra xem message có phải là JSON hợp lệ không
        /// </summary>
        public bool IsValidJson => Type == MessageType.Design_Request && ParsedData != null;
    }
}
