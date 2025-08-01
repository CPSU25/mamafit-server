namespace MamaFit.BusinessObjects.DTO.AI;

public class MeasurementPredictionDto
{
    public int WeekOfPregnancy { get; set; }
    public float Weight { get; set; }
    public float Neck { get; set; }
    public float Coat { get; set; }
    public float Bust { get; set; }
    public float ChestAround { get; set; }
    public float Stomach { get; set; }
    public float PantsWaist { get; set; }
    public float Thigh { get; set; }
    public float DressLength { get; set; }
    public float SleeveLength { get; set; }
    public float ShoulderWidth { get; set; }
    public float Waist { get; set; }
    public float LegLength { get; set; }
    public float Hip { get; set; }
        
    // Confidence scores cho từng measurement
    public Dictionary<string, float> ConfidenceScores { get; set; }
    public float OverallConfidence { get; set; }
        
    // Metadata
    public string ModelVersion { get; set; }
    public DateTime PredictedAt { get; set; }
}