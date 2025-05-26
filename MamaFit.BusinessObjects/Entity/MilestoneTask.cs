using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class MilestoneTask : BaseEntity
{
    public string? Name { get; set; }
    public string? TaskId { get; set; }
    public string? Description { get; set; }
    public MilestoneTaskStatus? MilestoneTaskStatus { get; set; }
    
    // Navigation properties
    public MaternityDressTask? MaternityDressTask { get; set; }
}