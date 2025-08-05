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
                query = query.Where(x => x.ComponentOptionPresets.Select(co => co.ComponentOption!.Name).Contains(search));
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
                .Include(x => x.ComponentOptionPresets)
                .ThenInclude(cop => cop.ComponentOption)
                .ThenInclude(co => co.Component)
                .Include(x => x.Style)
                .Where(p => !p.IsDeleted &&
                     componentOptionId.All(id => p.ComponentOptionPresets.Any(co => co.ComponentOption!.Id == id)))
                .ToListAsync();

            return presets;
        }

        public async Task<Preset> GetDefaultPresetByStyleId(string styleId)
        {
            var preset = await _dbSet
                .Include(x => x.ComponentOptionPresets)
                .ThenInclude(cop => cop.ComponentOption)
                .ThenInclude(co => co.Component)
                .Include(x => x.Style)
                .FirstOrDefaultAsync(x => x.StyleId == styleId && x.IsDefault && !x.IsDeleted && x.IsDefault);

            return preset;
        }

        public async Task<Preset> GetDetailById(string id)
        {
            var result = await _dbSet
                .Include(x => x.ComponentOptionPresets)
                .ThenInclude(cop => cop.ComponentOption)
                .ThenInclude(co => co.Component)
                .Include(x => x.OrderItems)
                .Include(x => x.Style)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return result;
        }

        public Task<List<Preset>> GetPresetByDesignRequestId(string designRequestId)
        {
            var presets = _dbSet
                .Include(x => x.ComponentOptionPresets)
                .ThenInclude(cop => cop.ComponentOption)
                .ThenInclude(co => co.Component)
                .Include(x => x.Style)
                .Where(x => x.DesignRequest.Id == designRequestId && !x.DesignRequest.IsDeleted)
                .ToListAsync();

            return presets;
        }

        public async Task<PaginatedList<Preset>> GetMostSelledPreset(int index, int pageSize, DateTime? startDate, DateTime? endDate, OrderStatus? filterBy)
        {
            var query = _dbSet.AsNoTracking()
                .Include(x => x.OrderItems).ThenInclude(x => x.Order)
                .OrderByDescending(x => x.OrderItems.Count()).Where(x => !x.IsDeleted);
            if (startDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= endDate.Value);
            }
            if (filterBy != null)
            {
                query.OrderByDescending(x => x.OrderItems.Any(x => x.Order.Status == filterBy));
            }

            return await query.GetPaginatedList(index, pageSize);
        }
    }
}
