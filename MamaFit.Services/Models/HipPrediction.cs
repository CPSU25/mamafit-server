using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class HipPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}