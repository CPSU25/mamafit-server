using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class PresetRepository : GenericRepository<Preset>, IPresetRepository
    {
        public PresetRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Preset>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(p => p.Style)
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.ComponentOptions.Select(co => co.Name).Contains(search));
            }

            query = sortBy switch
            {
                EntitySortBy.CREATED_AT_ASC => query.OrderBy(x => x.CreatedAt),
                EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            var paged = await GetPaging(query, index, pageSize);
            var list = paged.Items
                .ToList();

            return new PaginatedList<Preset>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }

        public async Task<List<Preset>> GetAllPresetByComponentOptionId(List<string> componentOptionId)
        {
            var presets = await _dbSet
                .Include(x => x.ComponentOptions)
                .Include(x => x.Style)
                .Where(p => !p.IsDeleted &&
                     componentOptionId.All(id => p.ComponentOptions.Any(co => co.Id == id)))
                .ToListAsync();

            return presets;
        }

        public async Task<Preset> GetDefaultPresetByStyleId(string styleId)
        {
            var preset = await _dbSet
                .Include(x => x.ComponentOptions)
                .Include(x => x.Style)
                .FirstOrDefaultAsync(x => x.StyleId == styleId && x.IsDefault && !x.IsDeleted && x.IsDefault);

            return preset;
        }

        public async Task<Preset> GetDetailById(string id)
        {
            var result = await _dbSet
                .Include(x => x.ComponentOptions)
                .Include(x => x.OrderItems)
                .Include(x => x.Style)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return result;
        }
    }
}
