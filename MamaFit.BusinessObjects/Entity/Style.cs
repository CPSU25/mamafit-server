using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Style : BaseEntity
    {
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Name { get; set; }
        public bool? IsCustom { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public GlobalStatus GlobalStatus { get; set; } = GlobalStatus.ACTIVE;
        public virtual ICollection<Preset>? Presets { get; set; } = [];
    }
}
