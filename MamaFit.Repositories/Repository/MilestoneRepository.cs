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
    public class MilestoneRepository : GenericRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Milestone>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
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

            var responseList = new PaginatedList<Milestone>
                (listResult, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);

            return responseList;
        }

        public async Task<Milestone> GetByIdDetailAsync(string id)
        {
            var milestone = await _dbSet
                .Include(x => x.MaternityDressTasks)
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(m => m.Id == id);

            return milestone;
        }
    }
}
