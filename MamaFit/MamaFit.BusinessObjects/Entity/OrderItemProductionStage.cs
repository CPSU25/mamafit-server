namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItemProductionStage : BaseEntity
    {
        public string? ProductionStageId { get; set; }
        public string? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public ProductionStage? ProductionStage { get; set; }
        public bool? IsCompelete { get; set; }
        public List<string>? Images { get; set; }
        public string? Note { get; set; }
    }
}
