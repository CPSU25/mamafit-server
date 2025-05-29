namespace MamaFit.BusinessObjects.DTO.AuthDto;

public class LoginRequestDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string NotificationToken { get; set; } = string.Empty;
}