using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressTask : BaseEntity
{
    public string? MilestoneId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DressTaskType? Type { get; set; }

    // Navigation properties
    public Milestone? Milestone { get; set; }
    public virtual ICollection<OrderItemTask>? OrderItemTasks { get; set; } = new List<OrderItemTask>();
}