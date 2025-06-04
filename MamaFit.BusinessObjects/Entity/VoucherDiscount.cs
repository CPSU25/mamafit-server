using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class VoucherDiscount : BaseEntity
    {
        public string? UserId { get; set; }
        public string? VoucherBatchId { get; set; }
        public string? Code { get; set; }
        public VoucherStatus? Status { get; set; }
        
        // Navigation properties
        public VoucherBatch? VoucherBatch { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
