using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressTask : BaseEntity
{
    public string? MilestoneId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int SequenceOrder { get; set; }
    public int EstimateTimeSpan { get; set; }

    // Navigation properties
    public Milestone? Milestone { get; set; }
    public virtual ICollection<OrderItemTask>? OrderItemTasks { get; set; } = new List<OrderItemTask>();
}