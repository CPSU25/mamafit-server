namespace MamaFit.AI.Infrastructure;

public class ModelMetrics
{
    public double MeanAbsoluteError { get; set; }
    public double MeanSquaredError { get; set; }
    public double RootMeanSquaredError { get; set; }
    public double RSquared { get; set; }
    public DateTime TrainedAt { get; set; }
    public int TrainingSampleCount { get; set; }
        
    public string GetSummary()
    {
        return $"R² Score: {RSquared:P}, RMSE: {RootMeanSquaredError:F2}";
    }
}