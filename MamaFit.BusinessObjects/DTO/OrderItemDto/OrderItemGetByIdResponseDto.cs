using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.PresetDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class OrderItemGetByIdResponseDto : OrderItemResponseDto
    {
        public PresetGetAllResponseDto? Preset { get; set; }
        public DesignResponseDto? DesisgnRequest { get; set; }
        public MaternityDressDetailResponseDto? MaternityDressDetail { get; set; }
        public List<MilestoneGetByIdResponseDto>? Milestones { get; set; }
    }
}
