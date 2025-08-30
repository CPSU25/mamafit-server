using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class VoucherBatch : BaseEntity
    {
        public string BatchName { get; set; }
        public string? BatchCode { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalQuantity { get; set; }
        public int? RemainingQuantity { get; set; }
        public DiscountType? DiscountType { get; set; }
        public int? DiscountValue { get; set; }
        public decimal? MinimumOrderValue { get; set; }
        public decimal? MaximumDiscountValue { get; set; }
        public bool? IsAutoGenerate { get; set; } = false;
        
        // Navigation properties
        public virtual ICollection<VoucherDiscount> VoucherDiscounts { get; set; } = [];
    }
}
