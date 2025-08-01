using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class AddOnOptionRepository : GenericRepository<AddOnOption>, IAddOnOptionRepository
{
    public AddOnOptionRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
    }

    public async Task<PaginatedList<AddOnOption>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy) 
    {
        var query = _dbSet.AsNoTracking()
            .Include(x => x.AddOn)
            .Include(x => x.Position)
            .Include(x => x.Size)
            .Where(a => a.IsDeleted.Equals(false));

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(a => a.Name.Contains(search) || a.Description.Contains(search));
        }

        query = sortBy switch
        {
            EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
            EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
            _ => query.OrderByDescending(u => u.CreatedAt)
        };

        return await GetPaging(query, index, pageSize);
    }
}