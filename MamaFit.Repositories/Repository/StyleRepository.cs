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
    public class StyleRepository : GenericRepository<Style>, IStyleRepository
    {
        public StyleRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Style>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search)) // Search
            {
                query = query.Where(u => u.Name!.Contains(search));
            }

            query = sortBy switch
            {
                
                
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize); // Paging

            var listStyle = pagedResult.Items
                .ToList();

            var responsePaginatedList = new PaginatedList<Style>(
                listStyle,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }

        public async Task<PaginatedList<Style>> GetAllByCategoryAsync(string categoryId, int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Where(c => !c.IsDeleted && c.CategoryId!.Equals(categoryId));

            if (!string.IsNullOrWhiteSpace(search)) // Search
            {
                query = query.Where(u => u.Name!.Contains(search));
            }

            query = sortBy switch
            {
                
                
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize); // Paging

            var listStyle = pagedResult.Items
                .ToList();

            var responsePaginatedList = new PaginatedList<Style>(
                listStyle,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }

        public async Task<Style> GetDetailById(string id)
        {
            var result = await _dbSet
                .Include(x => x.Presets)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return result!;
        }
    }
}