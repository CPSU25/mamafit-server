namespace MamaFit.BusinessObjects.DTO.UserDto;

public class UpdateUserRequestDto
{
    public string? UserName { get; set; } = string.Empty;
    public string? JobTitle { get; set; }
    public string? UserEmail { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? RoleId { get; set; }
}