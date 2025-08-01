using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class ThighPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}