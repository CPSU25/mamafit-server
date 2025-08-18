using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyDecisionItemDto
{
    public string OrderItemId { get; set; }
    public WarrantyRequestItemStatus Status { get; set; }
    public DestinationType DestinationType { get; set; }
    public decimal? Fee { get; set; }
    public decimal? ShippingFee { get; set; }
    public DateTime? EstimateTime { get; set; }
    public string? RejectedReason { get; set; }
}