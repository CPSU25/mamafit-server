using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity;

public class AddOn : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<AddOnOption>? AddOnOptions { get; set; } = new List<AddOnOption>();
}