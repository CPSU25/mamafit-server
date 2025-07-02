namespace MamaFit.BusinessObjects.Entity;

public class OrderItemServiceOption
{
    public string? OrderItemId { get; set; }
    public string? MaternityDressServiceOptionId { get; set; }
    public string? Value { get; set; } 
    
    //Navigation properties
    public OrderItem? OrderItem { get; set; } 
    public MaternityDressServiceOption? MaternityDressServiceOption { get; set; }
}