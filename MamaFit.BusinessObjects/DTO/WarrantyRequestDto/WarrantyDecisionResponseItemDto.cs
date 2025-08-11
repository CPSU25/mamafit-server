using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyDecisionResponseItemDto
{
    public string OrderItemId { get; set; } = default!;
    public WarrantyRequestItemStatus Status { get; set; } = default!; 
    public string? TrackingCode { get; set; }
    public object? GhtkCreateResponse{ get; set; }
    public string? GhtkCreateMessage { get; set; }
    public object? GhtkCancelResponse { get; set; }
    public string? GhtkCancelMessage { get; set; }
}