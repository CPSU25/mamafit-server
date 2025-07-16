using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class PositionRepository : GenericRepository<Position>, IPositionRepository
{
    public PositionRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
    
    public async Task<PaginatedList<Position>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
    {
        var query = _dbSet.AsNoTracking()
            .Where(p => p.IsDeleted.Equals(false));

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        query = sortBy switch
        {
            EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
            EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
            _ => query.OrderByDescending(u => u.CreatedAt) // default
        };

        return await GetPaging(query, index, pageSize);
    }
}