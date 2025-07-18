namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class AssignTaskToOrderItemRequestDto
    {
        public string? OrderItemId { get; set; }
        public List<string>? MilestoneIds { get; set; }
    }
}
