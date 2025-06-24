using MamaFit.BusinessObjects.DTO.MaternityDressSelectionDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressCustomizationDto
{
    public class CustomResponseDto
    {
        public string? Id { get; set; }
        public string? DesignRequestId { get; set; }
        public CustomizationEnum? CustomizationType { get; set; }
        public List<SelectionResponseDto> Selections { get; set; } = new List<SelectionResponseDto>();
        public float TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
