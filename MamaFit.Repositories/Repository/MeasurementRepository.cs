using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class MeasurementRepository : GenericRepository<Measurement>, IMeasurementRepository
{
    public MeasurementRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }

    public async Task<PaginatedList<Measurement>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);

        if (startDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= endDate.Value);
        }

        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<Measurement?> GetByIdAsync(string id)
    {
        return await GetByIdNotDeletedAsync(id);   
    }
    
    public async Task<Measurement?> GetLatestMeasurementByDiaryIdAsync(string diaryId)
    {
        return await _dbSet
            .Where(m => m.MeasurementDiaryId == diaryId && !m.IsDeleted)
            .OrderByDescending(m => m.WeekOfPregnancy)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> ValidateMeasurementExistenceAsync(string measurementDiaryId, int weekOfPregnancy)
    {
        return await _dbSet.AnyAsync(m => m.MeasurementDiaryId == measurementDiaryId && 
                                      m.WeekOfPregnancy == weekOfPregnancy && 
                                      !m.IsDeleted);
    }
}