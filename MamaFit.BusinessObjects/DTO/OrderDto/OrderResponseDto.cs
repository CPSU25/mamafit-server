using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.OrderDto;

public class OrderResponseDto : OrderBaseDto
{
    public string Id { get; set; }
    public Address? Address { get; set; } = new Address();
    public string? Code { get; set; }
    public decimal? DiscountSubtotal { get; set; }
    public decimal? DepositSubtotal { get; set; }
    public decimal? RemainingBalance { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}