using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemTaskDto
{
    public class OrderItemTaskUpdateRequestDto
    {
        public string? Note {  get; set; }
        public string? Image { get; set; }
        public OrderItemTaskStatus Status { get; set; }
    }
}
