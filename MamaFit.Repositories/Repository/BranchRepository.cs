using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Branch>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.Name.Contains(search));
            }

            query = sortBy switch
            {
                
                
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize); // Paging

            var listBranch = pagedResult.Items
                .ToList();

            var responsePaginatedList = new PaginatedList<Branch>(
                listBranch,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }
    }
}
