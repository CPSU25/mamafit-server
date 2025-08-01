using Microsoft.ML.Data;

namespace MamaFit.Services.Models;

public class MeasurementMLInput
{
    [LoadColumn(0)] public float Age { get; set; }
    [LoadColumn(1)] public float Height { get; set; }
    [LoadColumn(2)] public float PrePregnancyWeight { get; set; }
    [LoadColumn(3)] public float CurrentWeight { get; set; }
    [LoadColumn(4)] public float CurrentBust { get; set; }
    [LoadColumn(5)] public float CurrentWaist { get; set; }
    [LoadColumn(6)] public float CurrentHip { get; set; }
    [LoadColumn(7)] public float CurrentNeck { get; set; }
    [LoadColumn(8)] public float CurrentStomach { get; set; }
    [LoadColumn(9)] public float CurrentThigh { get; set; }
    [LoadColumn(10)] public float CurrentWeek { get; set; }
    [LoadColumn(11)] public float TargetWeek { get; set; }
    [LoadColumn(12)] public float WeeksDifference { get; set; }
        
    // Labels (what we predict) - for training
    [LoadColumn(13)] public float TargetWeight { get; set; }
    [LoadColumn(14)] public float TargetBust { get; set; }
    [LoadColumn(15)] public float TargetWaist { get; set; }
    [LoadColumn(16)] public float TargetHip { get; set; }
    [LoadColumn(17)] public float TargetNeck { get; set; }
    [LoadColumn(18)] public float TargetStomach { get; set; }
    [LoadColumn(19)] public float TargetThigh { get; set; }
}