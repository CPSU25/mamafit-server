namespace MamaFit.BusinessObjects.DTO.AI;

public class MeasurementPredictionDto
{
    public float Weight { get; set; }
    public float Bust { get; set; }
    public float Waist { get; set; }
    public float Hip { get; set; }
    public float ConfidenceScore { get; set; }
    public Dictionary<string, float> DetailedMeasurements { get; set; }
}