using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class WarrantyHistory : BaseEntity
    {
        public string? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public string? OrderId { get; set; }
        public Order? Order { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public WarrantyStatus? Status { get; set; }
        public DateTime? CompleteDate { get; set; }
    }
}
