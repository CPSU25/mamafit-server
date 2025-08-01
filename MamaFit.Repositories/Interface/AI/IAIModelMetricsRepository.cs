using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface.AI;

public interface IAIModelMetricsRepository : IGenericRepository<AIModelMetrics>
{
    Task<Dictionary<string, AIModelMetrics>> GetActiveModelsMetricsAsync();
    Task<AIModelMetrics> GetActiveModelByTypeAsync(string modelType);
    Task DeactivateOldModelsAsync(string modelType);
}