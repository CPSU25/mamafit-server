using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTaskDto
{
    public class MaternityDressTaskDetailResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DressTaskType? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = null;
        public string? UpdatedBy { get; set; } = string.Empty;
        public OrderItemTaskResponseDto? Detail { get; set; }
    }
}
