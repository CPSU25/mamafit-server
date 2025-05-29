namespace MamaFit.BusinessObjects.DTO.UserDto;

public class UserReponseDto
{
    public string? Id { get; set; }
    public string? Username { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? ProfilePicture { get; set; }
    public string? FullName { get; set; }
    public string? RoleName { get; set; }
    public bool IsVerify { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}