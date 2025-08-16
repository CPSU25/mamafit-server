using MamaFit.BusinessObjects.DTO.AddOnDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class OrderItemReadyToBuyRequestDto
    {
        public string? MaternityDressDetailId { get; set; }
        public int Quantity { get; set; }
        public List<AddOnOrderItemRequestDto>? Options { get; set; }
    }
}
