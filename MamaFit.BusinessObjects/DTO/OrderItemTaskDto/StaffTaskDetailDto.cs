using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto;

public class StaffTaskDetailDto
{
    public string? MaternityDressTaskId { get; set; }
    public string? Image { get; set; }
    public string? Note { get; set; }
    public OrderItemTaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string? MilestoneId { get; set; }
    public MaternityDressTaskResponseDto? MaternityDressTask { get; set; }
}