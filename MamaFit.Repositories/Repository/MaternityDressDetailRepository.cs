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
    public class MaternityDressDetailRepository : GenericRepository<MaternityDressDetail>, IMaternityDressDetailRepository
    {
        public MaternityDressDetailRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<MaternityDressDetail>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            query = sortBy switch
            {


                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await GetPaging(query, index, pageSize);
            var list = paged.Items
                .ToList();

            return new PaginatedList<MaternityDressDetail>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }

        public async Task<PaginatedList<MaternityDressDetail>> GetAllByMaternityDressId(string maternityDressId, int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet.Where(x => !x.IsDeleted && x.MaternityDressId!.Equals(maternityDressId));

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name!.Contains(search));
            }

            query = sortBy switch
            {


                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await GetPaging(query, index, pageSize);
            var list = paged.Items
                .ToList();

            return new PaginatedList<MaternityDressDetail>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }

        public async Task<MaternityDressDetail> GetDetailById(string maternityDetailId)
        {
            var response = await _dbSet
                .Include(x => x.MaternityDress)
                .FirstOrDefaultAsync(x => x.Id == maternityDetailId);

            return response;
        }
        
        public async Task<string?> GetLastSkuByPrefixAsync(string prefix)
        {
            return await _dbSet
                .Where(d => d.SKU != null && EF.Functions.Like(d.SKU!, prefix + "%"))
                .OrderByDescending(d => d.SKU)
                .Select(d => d.SKU)
                .FirstOrDefaultAsync();
        }

    }
}
