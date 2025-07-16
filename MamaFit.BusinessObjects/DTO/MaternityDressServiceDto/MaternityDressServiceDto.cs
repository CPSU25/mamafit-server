namespace MamaFit.BusinessObjects.DTO.MaternityDressServiceDto;

public class MaternityDressServiceDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<MaternityDressServiceOptionDto.MaternityDressServiceOptionDto>? MaternityDressServiceOptions { get; set; } = new List<MaternityDressServiceOptionDto.MaternityDressServiceOptionDto>();
}