using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyBranchRequestDto
{
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime EstimateTime { get; set; }
    public decimal? Fee { get; set; }
    public List<WarrantyRequestItemCreateDto> Items { get; set; } = new();
}