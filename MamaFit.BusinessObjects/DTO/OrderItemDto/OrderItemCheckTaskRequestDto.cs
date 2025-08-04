using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class OrderItemCheckTaskRequestDto
    {
        public List<string> MaternityDressTaskIds { get; set; }
        public string OrderItemId { get; set; }
        public OrderItemTaskStatus Status { get; set; }
    }
}
