using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Category>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var query = _dbSet
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search)) 
            {
                query = query.Where(u => u.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(u => u.Name),
                "name_desc" => query.OrderByDescending(u => u.Name),
                "createdat_asc" => query.OrderBy(u => u.CreatedAt),
                "createdat_desc" => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt) // default
            };

            var pagedResult = await GetPaging(query, index, pageSize); // Paging

            var listCategory = pagedResult.Items
                .ToList();

            var responsePaginatedList = new PaginatedList<Category>(
                listCategory,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }

        public async Task<Category> GetById(string id)
        {
            var category = await _dbSet
                .Include(c => c.Styles)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));
            return category!;
        }
    }
}
