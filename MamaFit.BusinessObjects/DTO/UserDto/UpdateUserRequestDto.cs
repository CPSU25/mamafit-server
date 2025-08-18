namespace MamaFit.BusinessObjects.DTO.UserDto;

public class UpdateUserRequestDto
{
    public string? UserName { get; set; } = string.Empty;
    public string? JobTitle { get; set; } 
    public string? UserEmail { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? FullName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? RoleId { get; set; }
}