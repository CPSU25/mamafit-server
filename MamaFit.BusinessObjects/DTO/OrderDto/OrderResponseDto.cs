using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.OrderDto;

public class OrderResponseDto : OrderBaseDto
{
    public string Id { get; set; }
    public OrderAddress Address { get; set; } = new OrderAddress();
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}