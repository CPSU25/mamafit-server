using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Component : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public GlobalStatus GlobalStatus { get; set; } = GlobalStatus.ACTIVE;

        //Navigation property
        public virtual ICollection<ComponentOption> Options { get; set; } = [];
        public virtual ICollection<Style> Styles { get; set; } = [];
    }
}
