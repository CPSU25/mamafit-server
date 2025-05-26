using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressTask : BaseEntity
{
    public string? MaternityDressCustomizationId { get; set; }
    public string? DesignRequestId { get; set; }
    public string? DesignerId { get; set; }
    public string? Description { get; set; }
    public DressTaskStatus? Status { get; set; }
    
    // Navigation properties
    public MaternityDressCustomization? MaternityDressCustomization { get; set; }
    public ICollection<MilestoneTask>? MilestoneTasks { get; set; }
    public DesignRequest? DesignRequest { get; set; }
    public ApplicationUser? Designer { get; set; }
}