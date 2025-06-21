using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class WarrantyHistory : BaseEntity
    {
        public string? WarrantyRequestId { get; set; }
        public WarrantyRequestStatus Status { get; set; }
        
        //Navigation property
        public WarrantyRequest? WarrantyRequest { get; set; }
    }
}
