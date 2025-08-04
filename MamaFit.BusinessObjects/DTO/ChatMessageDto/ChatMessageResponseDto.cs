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

        public object? ParsedData
        {
            get
            {
                if ((Type == MessageType.Design_Request || Type == MessageType.Preset) && !string.IsNullOrEmpty(Message))
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


        public List<string>? Images
        {
            get
            {
                if (ParsedData is JsonElement element && element.TryGetProperty("images", out var imagesElement) && imagesElement.ValueKind == JsonValueKind.Array)
                {
                    var imagesList = new List<string>();
                    foreach (var imageElement in imagesElement.EnumerateArray())
                    {
                        var imageUrl = imageElement.GetString();
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            imagesList.Add(imageUrl);
                        }
                    }
                    return imagesList;
                }
                return null;
            }
        }


        public string? PresetId
        {
            get
            {
                // Nếu Type là PRESET và có ParsedData, lấy từ JSON
                if (Type == MessageType.Preset)
                {
                    // Thử parse JSON trước
                    if (ParsedData is JsonElement element && element.TryGetProperty("presetId", out var presetId))
                    {
                        return presetId.GetString();
                    }
                    // Nếu không phải JSON, Message chính là PresetId (fallback cho legacy)
                    if (!string.IsNullOrEmpty(Message) && ParsedData == null)
                    {
                        return Message;
                    }
                }
                return null;
            }
        }


        public bool IsValidJson => (Type == MessageType.Design_Request || Type == MessageType.Preset) && ParsedData != null;
    }
}
