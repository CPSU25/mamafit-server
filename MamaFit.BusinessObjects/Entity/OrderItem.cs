using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItem : BaseEntity
    {
        public string? OrderId { get; set; }
        public string? MaternityDressDetailId { get; set; }
        public string? PresetId { get; set; }
        public ItemType? ItemType { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public int WarrantyNumber { get; set; } = 3;

        //Navigation property
        public Order? Order { get; set; }
        public MaternityDressDetail? MaternityDressDetail { get; set; }
        public Preset Preset { get; set; } = new Preset();
        public DesignRequest? DesignRequest { get; set; }
        public virtual ICollection<OrderItemTask>? OrderItemTasks { get; set; } = [];
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = [];
        public virtual ICollection<WarrantyRequest>? WarrantyRequests { get; set; } = [];
    }
}
