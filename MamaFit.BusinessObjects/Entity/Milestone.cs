using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class Milestone : BaseEntity
{
    public string? MaternityDressTaskId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    // Navigation properties
    public MaternityDressTask? MaternityDressTask { get; set; }
    public virtual ICollection<OrderItemTask> OrderItemTasks { get; set; } = new List<OrderItemTask>();
}