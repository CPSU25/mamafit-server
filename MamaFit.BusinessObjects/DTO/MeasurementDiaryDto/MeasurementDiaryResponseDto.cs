namespace MamaFit.BusinessObjects.DTO.MeasurementDiaryDto;

public class MeasurementDiaryResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; } 
    public int? NumberOfPregnancy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}