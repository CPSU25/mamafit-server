using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface.AI;

public interface IAIPredictionHistoryRepository : IGenericRepository<AIPredictionHistory>
{
    Task<AIPredictionHistory?> GetLatestPredictionAsync(string diaryId, int targetWeek);
    Task<List<AIPredictionHistory>> GetUserPredictionsAsync(string userId, int limit = 10);
    Task<double> CalculateAverageAccuracyAsync(string modelVersion);
    Task UpdateAccuracyScoreAsync(string predictionId, string actualMeasurementId);
}