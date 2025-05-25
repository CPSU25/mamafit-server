using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDressInspection : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public virtual ICollection<OrderItemInspection> ItemInspections { get; set; } = new List<OrderItemInspection>();
    }
}
