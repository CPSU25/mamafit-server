using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto;

public class OrderBaseDto
{
    public string? ParentOrderId { get; set; }
    public string? BranchId { get; set; }
    public string? UserId { get; set; }
    public string? AddressId { get; set; }
    public string? VoucherDiscountId { get; set; }
    public OrderType Type { get; set; }
    public OrderStatus? Status { get; set; }
    public float TotalAmount { get; set; }
    public float ShippingFee { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime? CanceledAt { get; set; }
    public string? CanceledReason { get; set; }
    public float SubTotalAmount { get; set; }
    public string? WarrantyCode { get; set; }
}