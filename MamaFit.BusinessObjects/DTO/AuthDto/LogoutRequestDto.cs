namespace MamaFit.BusinessObjects.DTO.AuthDto;

public class LogoutRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
    public string NotificationToken { get; set; } = string.Empty;
}