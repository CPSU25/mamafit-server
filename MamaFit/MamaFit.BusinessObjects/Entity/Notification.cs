using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Notification : BaseEntity
    {
        public string? NotificationTitle { get; set; }
        public string? NotificationContent { get; set; }
        public NotificationType? Type { get; set; }
        public string? ActionUrl { get; set; }
        public string? Metadata { get; set; }
        public bool? IsRead { get; set; } = false;
        public string? ReceiverId { get; set; }
    }
}
