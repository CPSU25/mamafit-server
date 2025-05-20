﻿using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItem : BaseEntity
    {
        public string? MaternityDressDetailId { get; set; }
        public string? OrderId { get; set; }
        public ItemType? ItemType { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime WarrantyDate { get; set; }
        public int WarrantyNumber { get; set; } = 3;
        
        //Navigation property
        public virtual ICollection<DesignOrder>  DesignOrders { get; set; } = new List<DesignOrder>();
        public virtual ICollection<MaternityDressCustomization>  MaternityDressCustomizations { get; set; } = new List<MaternityDressCustomization>();
        public virtual ICollection<OrderItemInspection> OrderItemInspections { get; set; } = [];
        public virtual ICollection<OrderItemProductionStage> OrderItemProductionStages { get; set; } = [];
        public virtual ICollection<Feedback> Feedbacks { get; set; } = [];
        public virtual ICollection<WarrantyRequest> WarrantyRequests { get; set; } = [];
        public Order? Order { get; set; }
        public MaternityDressDetail? MaternityDressDetail { get; set; }
    }
}
