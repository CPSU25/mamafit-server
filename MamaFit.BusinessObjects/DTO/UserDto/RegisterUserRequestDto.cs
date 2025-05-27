namespace MamaFit.BusinessObjects.DTO.UserDto;

public class RegisterUserRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}