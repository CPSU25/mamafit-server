namespace MamaFit.BusinessObjects.DTO.UserDto;

public class SendOTPRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}