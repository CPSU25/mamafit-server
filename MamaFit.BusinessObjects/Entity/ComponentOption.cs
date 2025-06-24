using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class ComponentOption : BaseEntity
    {
        public string? ComponentId { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public ComponentOptionType? ComponentOptionType { get; set; }
        public GlobalStatus GlobalStatus { get; set; } = GlobalStatus.ACTIVE;

        //Navigation property
        public virtual ICollection<MaternityDressSelection> MaternityDressSelections { get; set; } = new List<MaternityDressSelection>();
        public Component? Component { get; set; }
    }
}
