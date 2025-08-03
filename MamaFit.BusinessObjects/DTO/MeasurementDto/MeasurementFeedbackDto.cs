namespace MamaFit.BusinessObjects.DTO.MeasurementDto;

public class MeasurementFeedbackDto
{
    public string MeasurementId { get; set; }
    public Dictionary<string, float> ActualMeasurements { get; set; }
    public string? UserComments { get; set; }
    public int AccuracyRating { get; set; } // 1-5
}