using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository.AI;

public class AIModelMetricsRepository : GenericRepository<AIModelMetrics>, IAIModelMetricsRepository
{
    public AIModelMetricsRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }

    public async Task<Dictionary<string, AIModelMetrics>> GetActiveModelsMetricsAsync()
    {
        var activeModels = await _dbSet
            .Where(m => m.IsActive && !m.IsDeleted)
            .ToListAsync();

        return activeModels.ToDictionary(m => m.ModelType, m => m);
    }

    public async Task<AIModelMetrics> GetActiveModelByTypeAsync(string modelType)
    {
        var model = await _dbSet
            .FirstOrDefaultAsync(m => m.ModelType == modelType && m.IsActive && !m.IsDeleted);

        if (model == null)
        {
            throw new KeyNotFoundException($"No active model found for type: {modelType}");
        }

        return model;
    }

    public async Task DeactivateOldModelsAsync(string modelType)
    {
        var oldModels = await _dbSet
            .Where(m => m.ModelType == modelType && m.IsActive && !m.IsDeleted)
            .ToListAsync();

        foreach (var model in oldModels)
        {
            model.IsActive = false;
        }

        _context.UpdateRange(oldModels);
        await _context.SaveChangesAsync();
    }
}