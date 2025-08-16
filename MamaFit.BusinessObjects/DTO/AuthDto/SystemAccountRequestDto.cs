namespace MamaFit.BusinessObjects.DTO.AuthDto;

public class SystemAccountRequestDto
{
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
}