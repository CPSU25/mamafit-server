using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository.AI;

public class AIPredictionRepository : GenericRepository<AIPredictionHistory>, IAIPredictionRepository
{
    public AIPredictionRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }

    public async Task<List<AIPredictionHistory>> GetUserPredictionsAsync(string userId, int limit = 10)
    {
        return await _dbSet
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .OrderByDescending(p => p.PredictedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<AIPredictionHistory?> GetLatestPredictionAsync(string userId, string diaryId)
    {
        return await _dbSet
            .Where(p => p.UserId == userId && 
                        p.MeasurementDiaryId == diaryId && 
                        !p.IsDeleted)
            .OrderByDescending(p => p.PredictedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetAveragePredictionAccuracyAsync(string modelVersion)
    {
        return await _dbSet
            .Where(p => p.ModelVersion == modelVersion && !p.IsDeleted)
            .AverageAsync(p => (double)p.ConfidenceScore);
    }
}