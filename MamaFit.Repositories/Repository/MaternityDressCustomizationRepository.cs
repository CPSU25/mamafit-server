using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.Repositories.Repository
{
    public class MaternityDressCustomizationRepository : GenericRepository<MaternityDressCustomization>, IMaternityDressCustomizationRepository
    {
        public MaternityDressCustomizationRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<MaternityDressCustomization>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy)
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

            var responseList = new PaginatedList<MaternityDressCustomization>
                (listResult, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);

            return responseList;
        }

        public async Task<MaternityDressCustomization?> GetDetailById(string id)
        {
            var result = await _context.DressCustomizations
                .Include(x => x.MaternityDressSelections)
                .ThenInclude(x => x.ComponentOption)
                .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted);

            return result;
        }
    }
}
