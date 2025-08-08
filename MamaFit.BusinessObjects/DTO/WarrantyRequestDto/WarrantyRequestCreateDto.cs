using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyRequestCreateDto
{
    public string? AddressId { get; set; }
    public string? BranchId { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<WarrantyRequestItemCreateDto> Items { get; set; } = new();
}