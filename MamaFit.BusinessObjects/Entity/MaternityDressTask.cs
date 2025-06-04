using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressTask : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DressTaskType? Type { get; set; }
    
    // Navigation properties
    public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
}