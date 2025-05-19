using MamaFit.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class Order : BaseEntity
    {
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
    }
}
