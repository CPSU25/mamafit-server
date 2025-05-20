using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class VouchetBatch : BaseEntity
    {
        public string? BatchCode { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalQuantity { get; set; }
        public int? RemainingQuantity { get; set; }
        public string? DiscountType { get; set; }
        public int? DiscountPercentValue { get; set; }
        public float? MinimumOrderValue { get; set; }
        public float? MaximumDiscountValue { get; set; }
        public virtual ICollection<VoucherDiscount> VoucherDiscounts { get; set; } = [];
    }
}
