using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto;

public class StaffOrderItemTaskGroupDto
{
    public string? OrderItemId { get; set; }
    public OrderItemResponseDto? OrderItem { get; set; }
    public List<StaffTaskDetailDto> Tasks { get; set; } = new();
}
