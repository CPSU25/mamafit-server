using MamaFit.BusinessObjects.DTO.ComponentOptionDto;

namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetGetByIdResponseDto : PresetGetAllResponseDto
    {
        public List<ComponentOptionResponseDto>? ComponentOptions { get; set; }
    }
}
