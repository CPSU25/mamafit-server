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
    public class WarrantyRequestRepository : GenericRepository<WarrantyRequest>, IWarrantyRequestRepository
    {
        public WarrantyRequestRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(w => w.WarrantyHistories)
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.SKU.Contains(search));
            }

            query = sortBy switch
            {
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(x => x.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await GetPaging(query, index, pageSize);
            var list = paged.Items
                .ToList();

            return new PaginatedList<WarrantyRequest>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }
    }
}
