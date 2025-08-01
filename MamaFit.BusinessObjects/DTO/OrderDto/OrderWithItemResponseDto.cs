using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto;

public class OrderWithItemResponseDto
{
    public string Id { get; set; }
    public string? ParentOrderId { get; set; }
    public string? AddressId { get; set; }
    public string? BranchId { get; set; }
    public string? UserId { get; set; }
    public string? VoucherDiscountId { get; set; }
    public string? MeasurementId { get; set; }
    public bool? IsOnline { get; set; } = true;
    public OrderType Type { get; set; }
    public string? Code { get; set; }
    public OrderStatus? Status { get; set; }
    public float TotalAmount { get; set; } 
    public float ShippingFee { get; set; }
    public decimal? DiscountSubtotal { get; set; }
    public decimal? DepositSubtotal { get; set; }
    public decimal? RemainingBalance { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime? CanceledAt { get; set; }
    public string? CanceledReason { get; set; }
    public float SubTotalAmount { get; set; } 
    public string? WarrantyCode { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<OrderItemResponseDto> OrderItems { get; set; } 
}