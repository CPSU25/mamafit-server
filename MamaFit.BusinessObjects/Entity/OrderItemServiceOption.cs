namespace MamaFit.BusinessObjects.Entity;

public class OrderItemServiceOption
{
    public string? OrderItemId { get; set; }
    public string? AddOnOptionId { get; set; }
    public string? Value { get; set; } 
    
    //Navigation properties
    public OrderItem? OrderItem { get; set; } 
    public AddOnOption? AddOnOption { get; set; }
}