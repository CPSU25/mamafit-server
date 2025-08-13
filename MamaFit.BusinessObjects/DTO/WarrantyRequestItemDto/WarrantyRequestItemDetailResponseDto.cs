using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto
{
    public class WarrantyRequestItemDetailResponseDto
    {
        public string? OrderItemId { get; set; }
        public string? DestinationBranchId { get; set; }
        public string? TrackingCode { get; set; }
        public decimal? Fee { get; set; }
        public string? RejectedReason { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public List<string>? Videos { get; set; } = [];
        public string? Status { get; set; }
        public DateTime? EstimateTime { get; set; }
        public DestinationType DestinationType { get; set; }
        public int WarrantyRound { get; set; }
        public List<OrderWarrantyOnlyCode>? Orders {  get; set; }
        public List<WarrantyHistoryResponseDto>? Histories { get; set; }
    }
}
