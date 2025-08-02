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

    public async Task<List<Measurement>> GetAllWithDiariesAsync()
    {
        return await _dbSet
            .Include(m => m.MeasurementDiary)
            .Include(x => x.Orders)
            .Where(m => !m.IsDeleted && m.MeasurementDiary != null)
            .OrderBy(m => m.MeasurementDiaryId)
            .ThenBy(m => m.WeekOfPregnancy)
            .ToListAsync();
    }
    public async Task<PaginatedList<Measurement>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet.Include(x => x.Orders).Where(x => !x.IsDeleted);

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
        return await _dbSet.Include(x => x.MeasurementDiary)
            .Include(x => x.Orders)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Measurement?> GetLatestMeasurementByDiaryIdAsync(string diaryId)
    {
        return await _dbSet
            .Include(x => x.Orders)
            .Include(x => x.MeasurementDiary)
            .Where(m => m.MeasurementDiaryId == diaryId && !m.IsDeleted)
            .OrderByDescending(m => m.WeekOfPregnancy)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> ValidateMeasurementExistenceAsync(string MeasurementId, int weekOfPregnancy)
    {
        return await _dbSet.AnyAsync(m => m.MeasurementDiaryId == MeasurementId && 
                                      m.WeekOfPregnancy == weekOfPregnancy && 
                                      !m.IsDeleted);
    }
}