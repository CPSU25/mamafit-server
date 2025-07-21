using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class ComponentOption : BaseEntity
    {
        public string? ComponentId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public Tag? Tag { get; set; }
        public GlobalStatus GlobalStatus { get; set; } = GlobalStatus.ACTIVE;

        //Navigation property
        public virtual ICollection<ComponentOptionPreset>? ComponentOptionPresets { get; set; } = new List<ComponentOptionPreset>();
        public Component? Component { get; set; }
    }

    public class Tag
    {
        public List<string> ParentTag { get; set; } = new List<string>();
        public List<string> ChildTag { get; set; } = new List<string>();
    }
}
