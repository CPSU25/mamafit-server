using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class DesignRequestRepository : GenericRepository<DesignRequest>, IDesignRequestRepository
    {
        public DesignRequestRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<DesignRequest>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(d => d.User)
                .Include(d => d.OrderItem)
                .Where(d => !d.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d => d.Description!.Contains(search));
            }

            query = sortBy switch
            {
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var result = pagedResult.Items.ToList();
            return new PaginatedList<DesignRequest>(
                result,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }
    }
}
