using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class WeightPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}