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
    public class ComponentRepository : GenericRepository<Component>, IComponentRepository
    {
        public ComponentRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Component>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            query = sortBy switch
            {
                
                
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var listComponent = pagedResult.Items
                .ToList();

            return new PaginatedList<Component>(
                listComponent,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }

        public async Task<Component> GetById(string id)
        {
            var component = await _dbSet
                .Include(c => c.Options.Where(c => !c.IsDeleted))
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));
            return component!;
        }

        public async Task<List<Component>> GetComponentHavePresetByStyleId(string styleId)
        {
            var components = await _dbSet
                .Where(c => !c.IsDeleted)
                .Select(c => new Component
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Images = c.Images, 
                    GlobalStatus = c.GlobalStatus,
                    Options = c.Options
                        .Where(o => o.ComponentOptionPresets
                            .Any(cop => cop.Preset != null && cop.Preset.StyleId == styleId))
                        .Select(o => new ComponentOption
                        {
                            Id = o.Id,
                            Name = o.Name,
                            // các field khác của Option
                            ComponentOptionPresets = o.ComponentOptionPresets
                                .Where(cop => cop.Preset != null && cop.Preset.StyleId == styleId)
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            return components;
        }
    }
}
