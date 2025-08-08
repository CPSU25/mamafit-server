using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class WarrantyRequest : BaseEntity
    {
        public string? SKU { get; set; }
        public string? NoteInternal { get; set; } = null;
        public string? RejectedReason { get; set; }
        public RequestType RequestType { get; set; }
        public decimal? TotalFee { get; set; }
        public WarrantyRequestStatus? Status { get; set; }
        
        //Navigation property
        public virtual ICollection<WarrantyHistory> WarrantyHistories { get; set; } = [];
        public virtual ICollection<WarrantyRequestItem> WarrantyRequestItems { get; set; } = [];
    }
}
