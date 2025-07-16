namespace MamaFit.BusinessObjects.DTO.PositionDto;

public class PositionDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}