using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity;

public class Milestone : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<ItemType>? ApplyFor { get; set; }
    public int SequenceOrder { get; set; }
    // Navigation properties
    public virtual ICollection<MaternityDressTask>? MaternityDressTasks { get; set; } = [];
}