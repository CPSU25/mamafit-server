using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto
{
    public class OrderItemTaskGetByTokenResponse : OrderItemResponseDto
    {
        public MilestoneGetByIdOrderTaskResponseDto? Milestones { get; set; }
    }
}
