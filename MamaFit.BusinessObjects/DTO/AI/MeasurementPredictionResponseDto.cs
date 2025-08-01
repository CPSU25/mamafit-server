namespace MamaFit.BusinessObjects.DTO.AI;

public class MeasurementPredictionResponseDto
{
    public MeasurementPredictionDto Prediction { get; set; }
    public string Explanation { get; set; }
    public DateTime PredictedAt { get; set; }
}