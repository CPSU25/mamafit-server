using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class MyOrderStatusCount
    {
        public OrderStatus OrderStatus { get; set; }
        public int OrderNumber { get; set; }
    }
}
