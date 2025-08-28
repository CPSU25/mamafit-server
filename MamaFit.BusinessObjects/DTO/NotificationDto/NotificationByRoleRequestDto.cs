using System.ComponentModel.DataAnnotations;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.NotificationDto;

public class NotificationByRoleRequestDto
{
    [Required(ErrorMessage = "RoleIds is required")]
    public List<string> RoleIds { get; set; } = new();
    
    [Required(ErrorMessage = "NotificationTitle is required")]
    public string NotificationTitle { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "NotificationContent is required")]
    public string NotificationContent { get; set; } = string.Empty;
    
    public NotificationType Type { get; set; }
    public string? ActionUrl { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    
    /// <summary>
    /// Nếu true, chỉ gửi cho user active (IsVerify = true)
    /// </summary>
    public bool OnlyActiveUsers { get; set; } = true;
}