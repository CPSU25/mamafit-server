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

        public async Task<PaginatedList<MaternityDress>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(x => x.Style)
                .Include(x => x.Details.Where(x=> !x.IsDeleted))
                .Where(md => !md.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
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
            return await _dbSet.Include(x => x.Details)
                .Include(x => x.Style)
                .Include(x => x.Details.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }
    }
}
