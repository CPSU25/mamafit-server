using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class ComponentOption : BaseEntity
    {
        public string? ComponentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        
        //Navigation property
        public virtual ICollection<MaternityDressSelection> MaternityDressSelections { get; set; } = new List<MaternityDressSelection>();
        public Component? Component { get; set; }
    }
}
