namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyResponseDto : WarrantyBaseDto
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public int WarrantyRound { get; set; }
}