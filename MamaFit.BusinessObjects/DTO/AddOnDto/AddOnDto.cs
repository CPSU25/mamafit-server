namespace MamaFit.BusinessObjects.DTO.AddOnDto;

public class AddOnDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<AddOnOptionDto.AddOnOptionDto>? AddOnOptions { get; set; } = new List<AddOnOptionDto.AddOnOptionDto>();
}