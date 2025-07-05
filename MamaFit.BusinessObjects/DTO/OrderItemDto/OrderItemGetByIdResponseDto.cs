using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class OrderItemGetByIdResponseDto : OrderItemResponseDto
    {
        public DesignResponseDto? DesisgnRequest { get; set; }
        public MaternityDressDetailResponseDto? MaternityDressDetail { get; set; }
        public List<MilestoneGetByIdResponseDto>? Milestones { get; set; }
    }
}
