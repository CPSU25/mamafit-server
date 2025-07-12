using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.VoucherBatchDto;

public class VoucherBatchBaseDto
{
    public string? BatchName { get; set; }
    public string? BatchCode { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? TotalQuantity { get; set; }
    public DiscountType? DiscountType { get; set; }
    public int? DiscountValue { get; set; }
    public float? MinimumOrderValue { get; set; }
    public float? MaximumDiscountValue { get; set; }
}