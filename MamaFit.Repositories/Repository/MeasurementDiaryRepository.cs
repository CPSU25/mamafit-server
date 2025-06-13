using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

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
}