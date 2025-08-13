using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;

public class WarrantyRequestItemGetAllDto
{
    public string WarrantyRequestId { get; set; }
    public string OrderItemId { get; set; }
    public string? DestinationBranchId { get; set; }
    public string? TrackingCode { get; set; }
    public decimal? Fee { get; set; }
    public string? RejectedReason { get; set; }
    public string? Description { get; set; }
    public List<string>? Images { get; set; } = [];
    public string? Status { get; set; }
    public DateTime? EstimateTime { get; set; }
    public DestinationType DestinationType { get; set; }
    public int WarrantyRound { get; set; }
    
    // Navigation properties
    public WarrantyRequestGetAllDto? WarrantyRequest { get; set; }
    public WarrantyOrderWithOrderItemDto? OrderWithItems { get; set; }
    public BranchResponseDto? DestinationBranch { get; set; }
}

