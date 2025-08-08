using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyRequestCreateDto
{
    public string UserId { get; set; } = string.Empty;
    public string? AddressId { get; set; } = string.Empty;
    public string? BranchId { get; set; } = string.Empty;
    public string? MeasurementId { get; set; } = string.Empty;
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<WarrantyRequestItemCreateDto> Items { get; set; } = new();
}