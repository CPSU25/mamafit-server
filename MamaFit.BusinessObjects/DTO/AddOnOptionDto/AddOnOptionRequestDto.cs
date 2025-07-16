using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.AddOnOptionDto;

public class AddOnOptionRequestDto
{
    public string? AddOnId { get; set; }
    public string? PositionId { get; set; }
    public string? SizeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ItemServiceType? ItemServiceType { get; set; }
}