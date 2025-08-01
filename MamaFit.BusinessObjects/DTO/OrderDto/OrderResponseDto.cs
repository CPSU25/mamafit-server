using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.OrderDto;

public class OrderResponseDto : OrderBaseDto
{
    public string Id { get; set; }
    public string? AddressId { get; set; } 
    public string? Code { get; set; }
    public decimal? DiscountSubtotal { get; set; }
    public decimal? DepositSubtotal { get; set; }
    public decimal? RemainingBalance { get; set; }
    public decimal? TotalPaid { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public MeasurementDiaryResponseDto? MeasurementDiary { get; set; }
    public List<OrderItemResponseDto>? Items { get; set; }
}