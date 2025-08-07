using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;

public class WarrantyRequestItemCreateDto
{
    public string OrderItemId { get; set; }
    public string? Description { get; set; }
    public List<string>? Images { get; set; }
}