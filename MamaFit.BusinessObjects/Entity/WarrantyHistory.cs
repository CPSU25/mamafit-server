using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class WarrantyHistory : BaseEntity
    {
        public string? WarrantyRequestId { get; set; }
        public WarrantyRequestStatus Status { get; set; }
        public string? ActorId { get; set; }
        public string? Note { get; set; }

        //Navigation property
        public WarrantyRequest? WarrantyRequest { get; set; }
        public ApplicationUser? Actor { get; set; }
    }
}
