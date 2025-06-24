using MamaFit.BusinessObjects.DTO.MaternityDressSelectionDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressCustomizationDto
{
    public class CustomCreateRequestDto
    {
        public string? DesignRequestId { get; set; }
        public CustomizationEnum? CustomizationType { get; set; }
        public List<SelectionCreateRequestDto> Selections { get; set; } = new List<SelectionCreateRequestDto>();
    }
}
