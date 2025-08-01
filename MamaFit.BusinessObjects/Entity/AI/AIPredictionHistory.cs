using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity.AI;

public class AIPredictionHistory : BaseEntity
{
    public string UserId { get; set; }
    public string MeasurementDiaryId { get; set; }
    public int CurrentWeek { get; set; }
    public int TargetWeek { get; set; }
        
    // Store all predicted values
    public float PredictedWeight { get; set; }
    public float PredictedNeck { get; set; }
    public float PredictedCoat { get; set; }
    public float PredictedBust { get; set; }
    public float PredictedChestAround { get; set; }
    public float PredictedStomach { get; set; }
    public float PredictedPantsWaist { get; set; }
    public float PredictedThigh { get; set; }
    public float PredictedDressLength { get; set; }
    public float PredictedSleeveLength { get; set; }
    public float PredictedShoulderWidth { get; set; }
    public float PredictedWaist { get; set; }
    public float PredictedLegLength { get; set; }
    public float PredictedHip { get; set; }
        
    public float OverallConfidenceScore { get; set; }
    public string ModelVersion { get; set; }
    public DateTime PredictedAt { get; set; }
        
    // For tracking accuracy when actual measurement is recorded
    public string? ActualMeasurementId { get; set; }
    public float? AccuracyScore { get; set; }
        
    // Navigation
    public ApplicationUser User { get; set; }
    public MeasurementDiary MeasurementDiary { get; set; }
    public Measurement? ActualMeasurement { get; set; }
}