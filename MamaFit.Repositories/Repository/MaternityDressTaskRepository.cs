using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
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
    public class MaternityDressTaskRepository : GenericRepository<MaternityDressTask>, IMaternityDressTaskRepository
    {
        public MaternityDressTaskRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<MaternityDressTask>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet.AsNoTracking()
                .Where(a => a.IsDeleted.Equals(false));

            query = sortBy switch
            {
                EntitySortBy.NAME_ASC => query
                .OrderBy(u => u.Name),

                EntitySortBy.NAME_DESC => query
                .OrderByDescending(u => u.Name),

                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),

                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt) // default
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var listResult = pagedResult.Items
                .ToList();

            var responseList = new PaginatedList<MaternityDressTask>
                (listResult, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);

            return responseList;
        }

        public async Task<MaternityDressTask> GetByIdAsync(string id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return result;
        }
    }
}
