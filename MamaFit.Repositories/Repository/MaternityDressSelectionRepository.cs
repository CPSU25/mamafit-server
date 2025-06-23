using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class MaternityDressSelectionRepository : GenericRepository<MaternityDressSelection>,IMaternityDressSelectionRepository
    {
        public MaternityDressSelectionRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<MaternityDressSelection>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet.AsNoTracking()
                .Where(a => a.IsDeleted.Equals(false));

            query = sortBy switch
            {

                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),

                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt) // default
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var listResult = pagedResult.Items
                .ToList();

            var responseList = new PaginatedList<MaternityDressSelection>
                (listResult, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);

            return responseList;
        }

        public Task<MaternityDressSelection?> GetDetailById(string id)
        {
            var result = _dbSet
                .Include(x => x.ComponentOption)
                .Include(x => x.MaternityDressCustomization)
                .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted);
            return result;
        }
    }
}
