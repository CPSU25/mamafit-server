using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class WarrantyRequest : BaseEntity
    {
        public string? WarrantyOrderItemId { get; set; }
        public List<string>? Images { get; set; } = [];
        public string? Description { get; set; }
        public bool? IsFactoryError { get; set; } = null;
        public string? RejectedReason { get; set; }
        public float? Fee { get; set; }
        public WarrantyRequestStatus? Status { get; set; }
        public int WarrantyRound { get; set; }
        
        //Navigation property
        public virtual ICollection<WarrantyHistory> WarrantyHistories { get; set; } = [];
        public OrderItem? WarrantyOrderItem { get; set; }
    }
}
