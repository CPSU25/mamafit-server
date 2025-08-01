using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class BustPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}