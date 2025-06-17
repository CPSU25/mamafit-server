using MamaFit.BusinessObjects.DBContext;
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
        var query = _dbSet.Include(x => x.Measurements)
            .Where(x => x.Id == id && !x.IsDeleted);

        if (startDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= endDate.Value);
        }

        return await query.FirstOrDefaultAsync();
    }
}