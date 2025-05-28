namespace MamaFit.BusinessObjects.DTO.AuthDto;

public class LoginRequestDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string NotificationToken { get; set; } = string.Empty;
}