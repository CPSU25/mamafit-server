using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItem : BaseEntity
    {
        public string? OrderItemParentId { get; set; }
        public OrderItem? ParentOrderItem { get; set; }
        public string? MaternityDressDetailId { get; set; }
        public MaternityDressDetail? MaternityDressDetail { get; set; }
        public ItemType? ItemType { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<OrderItemInspection> ItemInspections { get; set; } = [];
        public virtual ICollection<OrderItemProductionStage> OrderItemProductionStages { get; set; } = [];
        public virtual ICollection<Feedback> Feedbacks { get; set; } = [];
        public virtual ICollection<WarrantyHistory> WarrantyHistories { get; set; } = [];
        public DateTime WarrantyDate { get; set; }
        public int WarrantyNumber { get; set; } = 3;
        public int RemainingWarranty { get; set; }
        public int WarrantyRound {  get; set; }
    }
}
