using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class StomachPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}