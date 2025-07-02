﻿using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderPresetCreateRequestDto
    {
        public string? PresetId { get; set; }
        public string? AddressId { get; set; }
        public string? BranchId { get; set; }
        public float ShippingFee { get; set; } = 0;
        public string? VoucherDiscountId { get; set; }
        public string? MeasurementDiaryId { get; set; }
        public bool IsOnline { get; set; } = true;
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}
