using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Preset : BaseEntity
    {
        public string? UserId { get; set; }
        public string? DesignRequestId { get; set; }
        public List<string>? Images { get; set; }
        public PresetType Type { get; set; }
        public ApplicationUser? User { get; set; }
        public DesignRequest? DesignRequest { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
        public virtual ICollection<ComponentOption> ComponentOptions { get; set; } = [];
    }
}
