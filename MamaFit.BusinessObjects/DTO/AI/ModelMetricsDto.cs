namespace MamaFit.BusinessObjects.DTO.AI;

public class ModelMetricsDto
{
    public string ModelType { get; set; } // Weight, Bust, Waist, etc.
    public double MeanAbsoluteError { get; set; }
    public double RootMeanSquaredError { get; set; }
    public double RSquared { get; set; }
    public int SampleCount { get; set; }
}