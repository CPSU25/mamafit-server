namespace MamaFit.BusinessObjects.DTO.VoucherBatchDto;

public class VoucherBatchResponseDto : VoucherBatchBaseDto
{
    public string Id { get; set; } = string.Empty;
    public int? RemainingQuantity { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}