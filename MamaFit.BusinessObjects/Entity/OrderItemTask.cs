using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class OrderItemTask
    {
        public string? UserId { get; set; }
        public string? OrderItemId { get; set; }
        public string? MaternityDressTaskId { get; set; }
        public string? Image { get; set; }
        public string? Note { get; set; }
        public OrderItemTaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = null;

        // Navigation Proptery
        public OrderItem? OrderItem { get; set; }
        public ApplicationUser? User { get; set; } // Desginer và Staff
        public MaternityDressTask? MaternityDressTask { get; set; }
    }
}
