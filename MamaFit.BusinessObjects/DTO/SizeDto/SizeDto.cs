namespace MamaFit.BusinessObjects.DTO.SizeDto;

public class SizeDto
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}