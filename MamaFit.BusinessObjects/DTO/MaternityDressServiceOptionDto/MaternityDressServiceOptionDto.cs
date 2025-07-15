using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressServiceOptionDto;

public class MaternityDressServiceOptionDto
{
    public string Id { get; set; }
    public string ? MaternityDressServiceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Position { get; set; }
    public string? Size { get; set; }
    public ItemServiceType? ItemServiceType { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}