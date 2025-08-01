using MamaFit.AI.Infrastructure;

namespace MamaFit.AI.Dto;

public class TrainingResult
{
    public bool Success { get; set; }
    public Dictionary<string, ModelMetrics> Metrics { get; set; } = new();
    public string Message { get; set; }
    public DateTime TrainedAt { get; set; }
}