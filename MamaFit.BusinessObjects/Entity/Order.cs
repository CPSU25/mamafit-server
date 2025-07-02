﻿using MamaFit.BusinessObjects.Base;
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
        public float TotalAmount { get; set; } // tổng tiền (bao gồm cả phí vận chuyển, giảm giá, thuế, v.v.)
        public float ShippingFee { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CanceledReason { get; set; }
        public float SubTotalAmount { get; set; } //gốc
        public string? WarrantyCode { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public Address? Address { get; set; } = new Address();
        public MeasurementDiary? MeasurementDiary { get; set; } = new MeasurementDiary();
        public VoucherDiscount? VoucherDiscount { get; set; }
        public Branch? Branch { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Transaction>? Transactions { get; set; } = [];
        public Order? ParentOrder { get; set; }
    }
}
