using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository
{
    public class MaternityDressDetailRepository : GenericRepository<MaternityDressDetail>, IMaternityDressDetailRepository
    {
        public MaternityDressDetailRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<MaternityDressDetail>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var query = _dbSet
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(x => x.Name),
                "name_desc" => query.OrderByDescending(x => x.Name),
                "createdat_asc" => query.OrderBy(x => x.CreatedAt),
                "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await GetPaging(query, index, pageSize);
            var list = paged.Items
                .ToList();

            return new PaginatedList<MaternityDressDetail>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }

        public async Task<PaginatedList<MaternityDressDetail>> GetAllByMaternityDressId(string maternityDressId, int index, int pageSize, string? search, string? sortBy)
        {
            var query = _dbSet.Where(x => !x.IsDeleted && x.MaternityDressId!.Equals(maternityDressId));

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name!.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(x => x.Name),
                "name_desc" => query.OrderByDescending(x => x.Name),
                "createdat_asc" => query.OrderBy(x => x.CreatedAt),
                "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await GetPaging(query, index, pageSize);
            var list = paged.Items
                .ToList();

            return new PaginatedList<MaternityDressDetail>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }
    }
}
