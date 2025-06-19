using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.VoucherDiscountDto;

public class VoucherDiscountBaseDto
{
    public string? UserId { get; set; }
    public string? VoucherBatchId { get; set; }
    public string? Code { get; set; }
    public VoucherStatus? Status { get; set; }
}