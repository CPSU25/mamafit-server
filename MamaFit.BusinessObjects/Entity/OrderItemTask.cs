namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItemTask
    {
        public string? MaternityDressTaskId { get; set; }
        public string? OrderItemId { get; set; }


        // Navigation Proptery
        public MaternityDressTask? MaternityDressTask { get; set; }
        public OrderItem? OrderItem { get; set; }
    }
}
