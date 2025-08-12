using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto
{
    public class OrderItemTaskGetByTokenResponse
    {
        public OrderItemResponseDto? OrderItem { get; set; }
        public MeasurementResponseDto? Measurement { get; set; }
        public string? OrderCode { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public string? AddressId { get; set; }
        public List<MilestoneGetByIdOrderTaskResponseDto>? Milestones { get; set; }
    }
}
