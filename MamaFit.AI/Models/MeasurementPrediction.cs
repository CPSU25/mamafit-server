namespace MamaFit.AI.Models;

public class MeasurementPrediction
{
    public float Weight { get; set; }
    public float Bust { get; set; }
    public float Waist { get; set; }
    public float Hip { get; set; }
    public float WeightConfidence { get; set; }
    public float BustConfidence { get; set; }
    public float WaistConfidence { get; set; }
    public float HipConfidence { get; set; }
        
    public float OverallConfidence => 
        (WeightConfidence + BustConfidence + WaistConfidence + HipConfidence) / 4;
        
    public Dictionary<string, float> GetDetailedMeasurements(float height)
    {
        return new Dictionary<string, float>
        {
            ["Stomach"] = Waist + 5f,
            ["PantsWaist"] = Waist - 5f,
            ["Coat"] = Bust + 5f,
            ["ChestAround"] = Bust + 3f,
            ["Thigh"] = (Hip + 5f) / 2f,
            ["Neck"] = height / 5f,
            ["ShoulderWidth"] = height / 4.3f,
            ["SleeveLength"] = height / 6.4f,
            ["DressLength"] = height * 0.66f,
            ["LegLength"] = height * 0.48f
        };
    }
}