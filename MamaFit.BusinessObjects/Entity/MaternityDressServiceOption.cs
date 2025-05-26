using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressServiceOption : BaseEntity
{
    public string ? MaternityDressServiceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Position { get; set; }
    public string? Size { get; set; }
    public ItemServiceType? ItemServiceType { get; set; }
    
    // Navigation properties
    public MaternityDressService? MaternityDressService { get; set; }
}