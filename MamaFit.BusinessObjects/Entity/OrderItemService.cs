using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class OrderItemService
{
    public string? OrderItemId { get; set; }
    public string? MaternityDressServiceId { get; set; }
    public string? Value { get; set; }
    
    // Navigation properties
    public virtual OrderItem? OrderItem { get; set; }
    public virtual MaternityDressService? MaternityDressService { get; set; }
}