using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.OrderDto;

public class OrderResponseDto : OrderBaseDto
{
    public string Id { get; set; }
    public Address Address { get; set; } = new Address();
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}