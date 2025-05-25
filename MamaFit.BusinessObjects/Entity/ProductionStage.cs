using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class ProductionStage : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsRequire { get; set; }
        public int SequenceOrder { get; set; }
        public virtual ICollection<OrderItemProductionStage> OrderItemProductionStages { get; set; } = new List<OrderItemProductionStage>();
    }
}
