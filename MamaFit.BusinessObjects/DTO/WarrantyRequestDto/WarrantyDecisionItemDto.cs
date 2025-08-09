using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyDecisionItemDto
{
    public string OrderItemId { get; set; }
    public WarrantyRequestItemStatus Status { get; set; }
    public string DestinationType { get; set; }
    public string? DestinationBranchId { get; set; }
    public decimal? Fee { get; set; }
    public DateTime? EstimateTime { get; set; }
    public string? RejectReason { get; set; }
}