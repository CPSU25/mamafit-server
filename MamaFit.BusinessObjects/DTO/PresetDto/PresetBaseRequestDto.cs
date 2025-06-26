
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetBaseRequestDto
    {
        public List<string>? Images { get; set; }
        public PresetType Type { get; set; }
    }
}
