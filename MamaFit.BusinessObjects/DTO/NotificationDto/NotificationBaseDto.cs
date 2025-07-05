using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.NotificationDto;

public class NotificationBaseDto
{
    public string? NotificationTitle { get; set; }
    public string? NotificationContent { get; set; }
    public NotificationType? Type { get; set; }
    public string? ActionUrl { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public string? ReceiverId { get; set; }
}

