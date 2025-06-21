using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class WarrantyHistoryRepository : GenericRepository<WarrantyHistory>, IWarrantyHistoryRepository
{
    public WarrantyHistoryRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
    
    public async Task<PaginatedList<WarrantyHistory>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet.AsNoTracking().Where(x => !x.IsDeleted);
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
}