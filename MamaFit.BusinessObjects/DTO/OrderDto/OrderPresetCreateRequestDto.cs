using MamaFit.BusinessObjects.DTO.AddOnDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderPresetCreateRequestDto
    {
        public string? PresetId { get; set; }
        public string? AddressId { get; set; }
        public string? BranchId { get; set; }
        public List<AddOnOrderItemRequestDto>? Options { get; set; }
        public decimal ShippingFee { get; set; } = 0;
        public string? VoucherDiscountId { get; set; }
        public string? MeasurementId { get; set; }
        public bool IsOnline { get; set; } = true;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentType PaymentType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}
