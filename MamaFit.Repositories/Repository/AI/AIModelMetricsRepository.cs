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
    
    public async Task<AIModelMetrics?> GetActiveModelMetricsAsync(string modelType)
    {
        return await _dbSet
            .Where(m => m.ModelType == modelType && !m.IsDeleted && m.IsActive)
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Dictionary<string, AIModelMetrics>> GetAllActiveMetricsAsync()
    {
        return await _dbSet
            .Where(m => !m.IsDeleted && m.IsActive)
            .ToDictionaryAsync(m => m.ModelType, m => m);
    }
    
    public async Task DeactivateOldMetricsAsync(string modelType)
    {
        var metrics = await _dbSet
            .Where(m => m.ModelType == modelType && !m.IsDeleted && m.IsActive)
            .ToListAsync();

        foreach (var metric in metrics)
        {
            metric.IsActive = false;
            metric.UpdatedAt = DateTime.UtcNow;
        }

        _context.UpdateRange(metrics);
        await _context.SaveChangesAsync();
    }
}