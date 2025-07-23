using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class ComponentOptionRepository : GenericRepository<ComponentOption>, IComponentOptionRepository
    {
        public ComponentOptionRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<ComponentOption>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(o => o.Component)
                .Where(o => !o.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(o => o.Name.Contains(search));
            }

            query = sortBy switch
            {
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var list = pagedResult.Items
                .ToList();

            return new PaginatedList<ComponentOption>(
                list,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }
    }
}
