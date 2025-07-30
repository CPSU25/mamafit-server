using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto;

public class AssignStaffDto
{
    public string? UserId { get; set; }
    public string? OrderItemId { get; set; }
    public string? MaternityDressTaskId { get; set; }
    public string? Image { get; set; }
    public string? Note { get; set; }
    public OrderItemTaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; } = null;
    
    public OrderItemResponseDto? OrderItem { get; set; }
    public MaternityDressTaskResponseDto? MaternityDressTask { get; set; }
}