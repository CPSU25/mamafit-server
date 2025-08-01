using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto
{
    public class OrderItemTaskGetByTokenResponse
    {
        public OrderItemResponseDto? OrderItem { get; set; }
        public MeasurementDiaryResponseDto? MeasurementDiary { get; set; }
        public string? OrderCode { get; set; }
        public string? AddressId { get; set; }
        public List<MilestoneGetByIdOrderTaskResponseDto>? Milestones { get; set; }
    }
}
