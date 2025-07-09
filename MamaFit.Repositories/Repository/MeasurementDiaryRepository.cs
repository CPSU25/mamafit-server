using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class MeasurementDiaryRepository : GenericRepository<MeasurementDiary>, IMeasurementDiaryRepository
{
    public MeasurementDiaryRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }
    
    public async Task<PaginatedList<MeasurementDiary>> GetAllDiariesAsync(int index, int pageSize, string? nameSearch)
    {
        var query = _dbSet
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nameSearch))
        {
            query = query.Where(x => x.Name!.Contains(nameSearch));
        }
        return await query.GetPaginatedList(index, pageSize);
    }
    
    public async Task<PaginatedList<MeasurementDiary>> GetByUserIdAsync(int index, int pageSize, string userId, string? nameSearch)
    {
        var query = _dbSet
            .Where(x => x.UserId == userId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nameSearch))
        {
            query = query.Where(x => x.Name!.Contains(nameSearch));
        }

        return await query.GetPaginatedList(index, pageSize);
    }
    
    public async Task<MeasurementDiary?> GetDiaryByIdAsync(string id, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet
            .Where(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.Measurements
                .Where(m =>
                    (!startDate.HasValue || m.CreatedAt >= startDate.Value) &&
                    (!endDate.HasValue || m.CreatedAt <= endDate.Value)
                ));

        return await query.FirstOrDefaultAsync();
    }

    public async Task SetActiveFalseForAllAsync(string userId)
    {
        var diaries = await _dbSet
            .Where(d => d.UserId == userId && d.IsActive)
            .ToListAsync();
        foreach (var diary in diaries) 
            diary.IsActive = false;
    }
}