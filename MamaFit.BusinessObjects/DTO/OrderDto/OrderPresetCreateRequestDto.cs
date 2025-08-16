using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderPresetCreateRequestDto
    {
        public string? AddressId { get; set; }
        public string? BranchId { get; set; }   
        public string? VoucherDiscountId { get; set; }
        public string? MeasurementId { get; set; }
        public decimal ShippingFee { get; set; } = 0;
        public bool IsOnline { get; set; } = true;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentType PaymentType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<PresetListIdRequestDto> Presets { get; set; } = new List<PresetListIdRequestDto>();
        
    }
}
