using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity;

public class Milestone : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public virtual ICollection<MaternityDressTask>? MaternityDressTasks { get; set; } = [];
}