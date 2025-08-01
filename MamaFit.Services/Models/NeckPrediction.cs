using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class NeckPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}