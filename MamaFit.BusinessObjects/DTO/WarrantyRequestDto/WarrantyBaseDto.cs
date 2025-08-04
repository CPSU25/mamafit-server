using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyBaseDto
    {
        public List<string>? Images { get; set; } = [];
        public string? Description { get; set; }
        public float? Fee { get; set; }
    }
}
