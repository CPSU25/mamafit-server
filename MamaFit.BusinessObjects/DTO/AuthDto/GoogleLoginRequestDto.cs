namespace MamaFit.BusinessObjects.DTO.AuthDto
{
    public class GoogleLoginRequestDto
    {
        public string JwtToken { get; set; } = null!;
        public string? NotificationToken { get; set; }
    }
}
