namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class AssignChargeToOrderItemRequestDto : AssignTaskToOrderItemRequestDto
    {
        public string? ChargeId { get; set; } // Charge ID to be assigned to the order item Designer or Staff
    }
}
