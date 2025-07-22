using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class AddOnOption : BaseEntity
{
    public string ? AddOnId { get; set; }
    public string? PositionId { get; set; }
    public string? SizeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ItemServiceType? ItemServiceType { get; set; }
    
    // Navigation properties
    public AddOn? AddOn { get; set; }
    public Position? Position { get; set; }
    public Size? Size { get; set; }
    public virtual ICollection<OrderItemAddOnOption>? OrderItemAddOnOptions { get; set; } = [];
}