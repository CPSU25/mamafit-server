namespace MamaFit.BusinessObjects.DTO.VoucherDiscountDto;

public class VoucherDiscountResponseDto : VoucherDiscountBaseDto
{
    public string Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}