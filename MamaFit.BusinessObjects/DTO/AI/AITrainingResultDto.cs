namespace MamaFit.BusinessObjects.DTO.AI;

public class AITrainingResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Dictionary<string, ModelMetricsDto> ModelMetrics { get; set; }
    public int TrainingSampleCount { get; set; }
    public DateTime TrainedAt { get; set; }
}