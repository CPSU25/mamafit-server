using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItemInspection : BaseEntity
    {
        public string OrderItemId { get; set; }
        public string MaternityDressInspectionId { get; set; }
        public OrderItem OrderItem { get; set; }
        public MaternityDressInspection MaternityDressInspection { get; set; }

        public bool IsChecked { get; set; } = false;
        public string? Image { get; set; }
        public string? Note { get; set; }
    }
}
