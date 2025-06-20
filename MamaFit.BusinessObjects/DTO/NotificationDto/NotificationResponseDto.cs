namespace MamaFit.BusinessObjects.DTO.NotificationDto;

public class NotificationResponseDto : NotificationBaseDto
{
    public string Id { get; set; }
    public bool IsRead { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}