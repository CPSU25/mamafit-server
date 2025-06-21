using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;

public class WarrantyHistoryBaseDto
{
    public string? WarrantyRequestId { get; set; }
    public WarrantyRequestStatus Status { get; set; }
}