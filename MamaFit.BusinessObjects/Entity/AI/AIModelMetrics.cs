using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity.AI;

public class AIModelMetrics : BaseEntity
{
    public string ModelType { get; set; } // Weight, Bust, Waist, Hip
    public string ModelVersion { get; set; }
    public double RSquared { get; set; }
    public double MeanAbsoluteError { get; set; }
    public double RootMeanSquaredError { get; set; }
    public int TrainingSampleCount { get; set; }
    public DateTime TrainedAt { get; set; }
    public bool IsActive { get; set; }
}