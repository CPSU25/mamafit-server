using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Preset : BaseEntity
    {
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public string? DesignRequestId { get; set; }
        public string? StyleId { get; set; }
        public List<string>? Images { get; set; }
        public bool IsDefault { get; set; } = false;
        public int? Weight { get; set; }
        public decimal? Price { get; set; }
        public PresetType Type { get; set; }
        public ApplicationUser? User { get; set; }
        public Style? Style { get; set; }
        public DesignRequest? DesignRequest { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
        public virtual ICollection<ComponentOptionPreset> ComponentOptionPresets { get; set; } = [];
    }
}
