using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface.AI;

public interface IAIPredictionRepository : IGenericRepository<AIPredictionHistory>
{
    Task<List<AIPredictionHistory>> GetUserPredictionsAsync(string userId, int limit = 10);
    Task<AIPredictionHistory?> GetLatestPredictionAsync(string userId, string diaryId);
    Task<double> GetAveragePredictionAccuracyAsync(string modelVersion);
}