using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyBaseDto
    {
        public List<string>? Images { get; set; } = [];
        public string? Description { get; set; }
        public float? Fee { get; set; }
        public WarrantyRequestStatus? Status { get; set; } = WarrantyRequestStatus.SUBMITTED;
        public int WarrantyRound { get; set; }
    }
}
