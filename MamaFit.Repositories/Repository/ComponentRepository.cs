using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.Repositories.Repository
{
    public class ComponentRepository : GenericRepository<Component>, IComponentRepository
    {
        public ComponentRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Component>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var query = _dbSet
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(c => c.Name),
                "name_desc" => query.OrderByDescending(c => c.Name),
                "createdat_asc" => query.OrderBy(c => c.CreatedAt),
                "createdat_desc" => query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };

            var pagedResult = await GetPagging(query, index, pageSize);

            var listComponent = pagedResult.Items
                .ToList();

            return new PaginatedList<Component>(
                listComponent,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }
    }
}
