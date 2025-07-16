using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto
{
    public class OrderItemTaskResponseDto
    {
        public string? Image { get; set; }
        public string? Note { get; set; }
        public string? ChargeId { get; set; }
        public string? ChargeName { get; set; }
        public OrderItemTaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } 
    }
}
