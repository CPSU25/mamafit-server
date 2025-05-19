namespace MamaFit.BusinessObjects.Entity
{
    public class Feedback : BaseEntity
    {
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public float? Rated { get; set; }
    }
}
