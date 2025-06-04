namespace MamaFit.BusinessObjects.DTO.MeasurementDiaryDto;

public class MeasurementDiaryRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int NumberOfPregnancy { get; set; } 
}