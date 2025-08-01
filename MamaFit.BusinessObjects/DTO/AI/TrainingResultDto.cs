namespace MamaFit.BusinessObjects.DTO.AI;

public class TrainingResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Dictionary<string, ModelMetricsDto> Metrics { get; set; }
    public DateTime TrainedAt { get; set; }
}

public class ModelMetricsDto
{
    public double MeanAbsoluteError { get; set; }
    public double RootMeanSquaredError { get; set; }
    public double RSquared { get; set; }
    public int TrainingSampleCount { get; set; }
}

public class AIStatusDto
{
    public bool MLReady { get; set; }
    public bool LLMAvailable { get; set; }
    public ModelMetricsDto MLMetrics { get; set; }
    public string ActiveLLMProvider { get; set; }
}