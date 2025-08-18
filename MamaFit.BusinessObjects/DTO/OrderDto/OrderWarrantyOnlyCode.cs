using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderWarrantyOnlyCode
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime? ReceivedAtBranch { get; set; }
        public List<OrderItemGetByIdResponseDto>? OrderItems { get; set; }
    }
}
