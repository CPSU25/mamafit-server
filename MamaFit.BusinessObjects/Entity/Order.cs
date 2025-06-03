using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Order : BaseEntity
    {
        public string? PanrentOrderId { get; set; }
        public string? BranchId { get; set; }
        public string? UserId { get; set; }
        public string? LocationId { get; set; }
        public string? VoucherDiscountId { get; set; }
        public OrderType Type { get; set; }
        public string? Code { get; set; }
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

        // Navigation properties
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public Address Location { get; set; } = new Address();
        public MeasurementDiary MeasurementDiary { get; set; } = new MeasurementDiary();
        public VoucherDiscount? VoucherDiscount { get; set; }
        public Branch? Branch { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Transaction>? Transactions { get; set; } = [];
        public Order? ParentOrder { get; set; }
    }
}
