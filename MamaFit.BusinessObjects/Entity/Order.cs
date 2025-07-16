using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Order : BaseEntity
    {
        public string? ParentOrderId { get; set; }
        public string? AddressId { get; set; }
        public string? BranchId { get; set; }
        public string? UserId { get; set; }
        public string? VoucherDiscountId { get; set; }
        public string? MeasurementDiaryId { get; set; }
        public bool? IsOnline { get; set; } = true;
        public OrderType Type { get; set; }
        public string? Code { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal? DiscountSubtotal { get; set; }
        public decimal? DepositSubtotal { get; set; }
        public decimal? RemainingBalance { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CanceledReason { get; set; }
        public decimal? SubTotalAmount { get; set; } 
        public string? WarrantyCode { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Address? Address { get; set; }
        public MeasurementDiary? MeasurementDiary { get; set; }
        public VoucherDiscount? VoucherDiscount { get; set; }
        public Branch? Branch { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Transaction>? Transactions { get; set; } = [];
        public Order? ParentOrder { get; set; }
    }
}
