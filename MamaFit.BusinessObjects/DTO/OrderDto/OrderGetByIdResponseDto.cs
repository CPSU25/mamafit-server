using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderGetByIdResponseDto : OrderResponseDto
    {
        public List<OrderItemResponseDto>? Items { get; set; }
    }
}
