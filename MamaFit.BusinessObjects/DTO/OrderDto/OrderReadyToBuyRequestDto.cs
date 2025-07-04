using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderReadyToBuyRequestDto
    {
        public string? UserId { get; set; }
        public string? AddressId { get; set; }
        public string? BranchId { get; set; }
        public string? VoucherDiscountId { get; set; }
        public string? MeasurementDiaryId { get; set; }
        public decimal ShippingFee { get; set; } = 0;
        public bool IsOnline { get; set; } = true;
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<OrderItemReadyToBuyRequestDto> OrderItems { get; set; } = [];
    }
}
