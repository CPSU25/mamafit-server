using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Component : BaseEntity
    {
        public string? StyleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        
        //Navigation property
        public virtual ICollection<ComponentOption> Option { get; set; } = [];
        public Style? Style { get; set; }
    }
}
