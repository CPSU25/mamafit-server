using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
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

        public async Task<PaginatedList<DesignRequest>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var query = _dbSet
                .Include(d => d.User)
                .Include(d => d.OrderItem)
                .Where(d => !d.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d => d.Description!.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "createdat_asc" => query.OrderBy(d => d.CreatedAt),
                "createdat_desc" => query.OrderByDescending(d => d.CreatedAt),
                _ => query.OrderByDescending(d => d.CreatedAt)
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
