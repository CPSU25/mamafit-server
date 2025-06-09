using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
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

        public async Task<PaginatedList<ComponentOption>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(o => o.Component)
                .Where(o => !o.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(o => o.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(o => o.Name),
                "name_desc" => query.OrderByDescending(o => o.Name),
                "createdat_asc" => query.OrderBy(o => o.CreatedAt),
                "createdat_desc" => query.OrderByDescending(o => o.CreatedAt),
                _ => query.OrderByDescending(o => o.CreatedAt),
            };

            var pagedResult = await GetPagging(query, index, pageSize);

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
