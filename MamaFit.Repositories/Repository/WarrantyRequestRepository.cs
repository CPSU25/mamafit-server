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
    public class WarrantyRequestRepository : GenericRepository<WarrantyRequest>, IWarrantyRequestRepository
    {
        public WarrantyRequestRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(w => w.WarrantyHistories)
                .Include(x => x.WarrantyRequestItems).ThenInclude(x => x.OrderItem).ThenInclude(x => x.Order).ThenInclude(x => x.User).ThenInclude(x => x.Role)
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.SKU.Contains(search));
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

            return new PaginatedList<WarrantyRequest>(list, paged.TotalCount, paged.PageNumber, pageSize);
        }

        public async Task<List<WarrantyRequest>> GetAllWarrantyRequestByOrderId(string orderId)
        {
            var result = _dbSet
                .Include(x => x.WarrantyRequestItems)
                .ThenInclude(x => x.OrderItem)
                .ThenInclude(x => x.Order)
                .Include(x => x.WarrantyRequestItems)
                .ThenInclude(x => x.OrderItem)
                .ThenInclude(x => x.ParentOrderItem)
                .Where(x => !x.IsDeleted)
                .Include(x => x.WarrantyRequestItems)
                .ThenInclude(x => x.OrderItem)
                .ThenInclude(x => x.Preset);
            return await result.ToListAsync();// nhận orderId, mảng orderItem và orderCode gốc của orderId 
        }

        public async Task<WarrantyRequest> GetDetailById(string warrantyId)
        {
            var result = await _dbSet
                .Include(x => x.WarrantyRequestItems).ThenInclude(x => x.OrderItem).ThenInclude(x => x.Order)
                .Include(x => x.WarrantyRequestItems).ThenInclude(x => x.OrderItem).ThenInclude(x => x.Preset).ThenInclude(x => x.Style)
                .Include(x => x.WarrantyHistories)
                .FirstOrDefaultAsync(x => !x.IsDeleted);

            return result;
        }
    }
}
