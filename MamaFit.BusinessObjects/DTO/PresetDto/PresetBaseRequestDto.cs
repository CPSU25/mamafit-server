
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetBaseRequestDto
    {
        public List<string>? Images { get; set; }
        public string? SKU { get; set; }
        public PresetType Type { get; set; }
        public bool IsDefault { get; set; } = false;
        public float Price { get; set; } 
    }
}
