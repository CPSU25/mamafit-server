using Microsoft.ML.Data;

namespace MamaFit.AI.Models;

public class MeasurementData
{
    [LoadColumn(0)]
    public float UserId { get; set; }
        
    [LoadColumn(1)]
    public float Age { get; set; }
        
    [LoadColumn(2)]
    public float Height { get; set; }
        
    [LoadColumn(3)]
    public float PrePregnancyWeight { get; set; }
        
    [LoadColumn(4)]
    public float CurrentWeight { get; set; }
        
    [LoadColumn(5)]
    public float CurrentBust { get; set; }
        
    [LoadColumn(6)]
    public float CurrentWaist { get; set; }
        
    [LoadColumn(7)]
    public float CurrentHip { get; set; }
        
    [LoadColumn(8)]
    public float CurrentWeek { get; set; }
        
    [LoadColumn(9)]
    public float TargetWeek { get; set; }
        
    // Labels (what we want to predict)
    [LoadColumn(10)]
    public float TargetWeight { get; set; }
        
    [LoadColumn(11)]
    public float TargetBust { get; set; }
        
    [LoadColumn(12)]
    public float TargetWaist { get; set; }
        
    [LoadColumn(13)]
    public float TargetHip { get; set; }
}