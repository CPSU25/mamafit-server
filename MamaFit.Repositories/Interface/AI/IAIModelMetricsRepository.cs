using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface.AI;

public interface IAIModelMetricsRepository : IGenericRepository<AIModelMetrics>
{
    Task<AIModelMetrics> GetActiveModelMetricsAsync(string modelType);
    Task<Dictionary<string, AIModelMetrics>> GetAllActiveMetricsAsync();
    Task DeactivateOldMetricsAsync(string modelType);
}