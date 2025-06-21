namespace MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;

public class WarrantyHistoryResponseDto
{
    public string Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}