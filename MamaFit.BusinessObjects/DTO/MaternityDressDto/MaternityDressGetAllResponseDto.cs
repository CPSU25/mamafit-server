using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressDto;

public class MaternityDressGetAllResponseDto
{
    public string? Id { get; set; }
    public string? StyleName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public List<string> Images { get; set; } = new List<string>();
    public List<float> Price { get; set; } = new List<float>();
    public GlobalStatus GlobalStatus { get; set; }
    public string? Slug { get; set; }
    public int SoldCount { get; set; }
    public int FeedbackCount { get; set; }
    public decimal AverageRating { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}