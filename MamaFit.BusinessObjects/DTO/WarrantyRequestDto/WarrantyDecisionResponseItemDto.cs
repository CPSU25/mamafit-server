using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyDecisionResponseItemDto
{
    public string OrderItemId { get; set; } = default!;
    public WarrantyRequestItemStatus Status { get; set; } = default!; 
    public string? TrackingCode { get; set; }
}