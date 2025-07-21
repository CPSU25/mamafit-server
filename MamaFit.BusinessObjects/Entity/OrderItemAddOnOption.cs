namespace MamaFit.BusinessObjects.Entity;

public class OrderItemAddOnOption
{
    public string? OrderItemId { get; set; }
    public string? AddOnOptionId { get; set; }
    public string? Value { get; set; } //Description
    
    //Navigation properties
    public OrderItem? OrderItem { get; set; } 
    public AddOnOption? AddOnOption { get; set; }
}