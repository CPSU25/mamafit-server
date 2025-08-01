using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity.AI;

public class AIModelMetrics : BaseEntity
{
    public string ModelType { get; set; } // "Weight", "Bust", "Waist", etc.
    public string ModelVersion { get; set; }
    public double MeanAbsoluteError { get; set; }
    public double RootMeanSquaredError { get; set; }
    public double RSquared { get; set; }
    public int TrainingSampleCount { get; set; }
    public DateTime TrainedAt { get; set; }
    public bool IsActive { get; set; }
    public string TrainingDataSummary { get; set; } // JSON with training stats
}