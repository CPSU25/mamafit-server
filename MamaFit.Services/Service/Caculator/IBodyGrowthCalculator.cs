namespace MamaFit.Services.Service.Caculator;

public interface IBodyGrowthCalculator
{
    float CalculateBust(float baseBust, int startWeek, int endWeek);
    float CalculateWaist(float baseWaist, int startWeek, int endWeek);
    float CalculateHip(float baseHip, int startWeek, int endWeek);
    float CalculateWeight(float baseWeight, int startWeek, int endWeek);
}