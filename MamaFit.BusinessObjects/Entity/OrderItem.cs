using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItem : BaseEntity
    {
        public string? OrderId { get; set; }
        public string? ParentOrderItemId { get; set; }
        public string? MaternityDressDetailId { get; set; }
        public string? PresetId { get; set; }
        public ItemType? ItemType { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? WarrantyDate { get; set; }

        //Navigation property
        public Order? Order { get; set; }
        public OrderItem? ParentOrderItem { get; set; }
        public MaternityDressDetail? MaternityDressDetail { get; set; }
        public Preset? Preset { get; set; }
        public DesignRequest? DesignRequest { get; set; }
        public virtual ICollection<OrderItemAddOnOption>? OrderItemAddOnOptions { get; set; } = [];
        public virtual ICollection<OrderItemTask>? OrderItemTasks { get; set; } = [];
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = [];
        public virtual ICollection<WarrantyRequest>? WarrantyRequests { get; set; } = [];
    }
}
