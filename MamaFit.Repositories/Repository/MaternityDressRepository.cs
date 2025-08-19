using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class MaternityDressRepository : GenericRepository<MaternityDress>, IMaternityDressRepository
    {
        public MaternityDressRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<MaternityDress>> GetAllAsync(int index, int pageSize, string? search, string styleId, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(x => x.Style)
                .Include(x => x.Details.Where(x => !x.IsDeleted))
                .ThenInclude(x => x.OrderItems).ThenInclude(x => x.Feedbacks)
                .Include(x => x.Details.Where(x => !x.IsDeleted))
                .ThenInclude(x => x.OrderItems).ThenInclude(x => x.Order)
                .Where(md => !md.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.Name!.ToLower().Contains(search.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(styleId))
                query = query.Where(x => x.StyleId == styleId);

            query = sortBy switch
            {
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.UpdatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize); // Paging

            var listMaternityDress = pagedResult.Items
                .ToList();

            var responsePaginatedList = new PaginatedList<MaternityDress>(
                listMaternityDress,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }

        public async Task<MaternityDress?> GetById(string id)
        {
            return await _dbSet.Include(x => x.Details).ThenInclude(x => x.OrderItems).ThenInclude(x => x.Order)
                .Include(x => x.Style)
                .Include(x => x.Details.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }
    }
}
