using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity.AI;

public class AIPredictionHistory : BaseEntity
{
    public string UserId { get; set; }
    public string MeasurementDiaryId { get; set; }
    public int CurrentWeek { get; set; }
    public int TargetWeek { get; set; }
    public float PredictedWeight { get; set; }
    public float PredictedBust { get; set; }
    public float PredictedWaist { get; set; }
    public float PredictedHip { get; set; }
    public float ConfidenceScore { get; set; }
    public string ModelVersion { get; set; }
    public DateTime PredictedAt { get; set; }
        
    // Navigation
    public ApplicationUser User { get; set; }
    public MeasurementDiary MeasurementDiary { get; set; }
}