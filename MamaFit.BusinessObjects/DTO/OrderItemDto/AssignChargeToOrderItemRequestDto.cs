namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class AssignChargeToOrderItemRequestDto
    {
        public string? ChargeId { get; set; } // Charge ID to be assigned to the order item Designer or Staff
        public string? OrderItemId { get; set; }
        public string? MilestoneId { get; set; }
    }
}
