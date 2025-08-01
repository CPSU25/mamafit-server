using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.AddOnOptionDto;

public class AddOnOptionDto
{
    public string Id { get; set; }
    public string ? AddOnId { get; set; }
    public string? PositionId { get; set; }
    public string? SizeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ItemServiceType? ItemServiceType { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public PositionDto.PositionDto? Position { get; set; }
    public SizeDto.SizeDto? Size { get; set; }
}