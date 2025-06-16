namespace MamaFit.Services.Service.Caculator;

public class BodyGrowthCalculator : IBodyGrowthCalculator
{
    private float ApplyGrowth(float baseValue, int startWeek, int endWeek, Func<int, float> growthPerWeek)
    {
        float result = baseValue;
        for (int week = startWeek + 1; week <= endWeek; week++)
            result += growthPerWeek(week);
        return result;
    }

    public float CalculateBust(float baseBust, int startWeek, int endWeek)
        => ApplyGrowth(baseBust, startWeek, endWeek, week =>
            week <= 12 ? 0f : week <= 26 ? 1.0f : 1.5f);

    public float CalculateWaist(float baseWaist, int startWeek, int endWeek)
        => ApplyGrowth(baseWaist, startWeek, endWeek, week =>
            week <= 12 ? 0f : week <= 26 ? 1.5f : 2.0f);

    public float CalculateHip(float baseHip, int startWeek, int endWeek)
        => ApplyGrowth(baseHip, startWeek, endWeek, week =>
            week <= 12 ? 0f : week <= 26 ? 1.0f : 1.2f);

    public float CalculateWeight(float baseWeight, int startWeek, int endWeek)
        => ApplyGrowth(baseWeight, startWeek, endWeek, week =>
            week <= 12 ? 0.1f : week <= 26 ? 0.4f : 0.6f);
}