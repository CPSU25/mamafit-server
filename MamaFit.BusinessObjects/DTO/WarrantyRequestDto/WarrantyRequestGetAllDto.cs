using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyRequestGetAllDto
{
    public string Id { get; set; }
    public string? SKU { get; set; }
    public string? NoteInternal { get; set; } = null;
    public RequestType RequestType { get; set; }
    public string? RejectedReason { get; set; }
    public decimal? TotalFee { get; set; }
    public WarrantyRequestStatus? Status { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}