using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class GetDetailDto
{
    public string Id { get; set; }
    public string? WarrantyOrderItemId { get; set; }
    public string? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public List<string>? Images { get; set; } = [];
    public string? Description { get; set; }
    public bool? IsFactoryError { get; set; } = null;
    public string? RejectedReason { get; set; }
    public float? Fee { get; set; }
    public WarrantyRequestStatus? Status { get; set; }
    public int WarrantyRound { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}