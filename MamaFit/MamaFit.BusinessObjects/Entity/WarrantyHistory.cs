using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class WarrantyHistory : BaseEntity
    {
        public string? WarrantyRequestId { get; set; }
        public WarrantyRequest? WarrantyRequest { get; set; }
        public WarrantyRequestStatus Status { get; set; }
    }
}
