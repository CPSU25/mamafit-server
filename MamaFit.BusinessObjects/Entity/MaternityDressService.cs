using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressService : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<MaternityDressServiceOption>? MaternityDressServiceOptions { get; set; } = new List<MaternityDressServiceOption>();
}