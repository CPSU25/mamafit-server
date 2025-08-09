using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyDecisionResponseDto
{
    public WarrantyRequestStatus RequestStatus { get; set; }
    public List<WarrantyDecisionResponseItemDto> Items { get; set; } = new();
}