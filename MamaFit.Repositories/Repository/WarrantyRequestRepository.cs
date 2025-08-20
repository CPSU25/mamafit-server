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
        public WarrantyRequestRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(
            context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestAsync(int index, int pageSize,
            string? search, EntitySortBy? sortBy)
        {
            var query = _dbSet
                .Include(w => w.WarrantyHistories)
                .Include(x => x.WarrantyRequestItems).ThenInclude(x => x.OrderItem).ThenInclude(x => x.Order)
                .ThenInclude(x => x.User).ThenInclude(x => x.Role)
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

        public async Task<List<WarrantyRequest>> GetAllWarrantyRequest()
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
            return await result.ToListAsync(); // nhận orderId, mảng orderItem và orderCode gốc của orderId 
        }

        public async Task<WarrantyRequest?> GetDetailById(string warrantyId)
        {
            var result = await _dbSet
                .Include(x => x.WarrantyRequestItems)
                .ThenInclude(wri => wri.OrderItem)
                .ThenInclude(oi => oi.Order)
                .ThenInclude(o => o.User)
                .Include(x => x.WarrantyRequestItems)
                .ThenInclude(wri => wri.OrderItem)
                .ThenInclude(oi => oi.ParentOrderItem)
                .ThenInclude(poi => poi.Order)
                .Include(x => x.WarrantyRequestItems)
                .ThenInclude(wri => wri.OrderItem)
                .ThenInclude(oi => oi.Preset)
                .ThenInclude(p => p.Style)
                .Include(x => x.WarrantyHistories)
                .SingleOrDefaultAsync(x => !x.IsDeleted && x.Id == warrantyId);
            return result;
        }
        
        public async Task<List<WarrantyRequest>> GetFeeWarrantyRequestsByOrderIdAsync(string orderId)
        {
            // Lấy các WR có bất kỳ item thuộc orderId này,
            // loại FEE, và item đang APPROVED về FACTORY (chưa IN_TRANSIT)
            return await _dbSet
                .Include(w => w.WarrantyRequestItems)
                .ThenInclude(i => i.OrderItem)
                .Where(w => w.RequestType == RequestType.FEE &&
                            w.WarrantyRequestItems.Any(i =>
                                i.OrderItem.OrderId == orderId &&
                                i.Status == WarrantyRequestItemStatus.APPROVED &&
                                i.DestinationType == DestinationType.FACTORY))
                .ToListAsync();
        }

        public Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestOfBranchAsync(int index, int pageSize, string? search, EntitySortBy? sortBy, string branchId)
        {
            var query = _dbSet
                .Include(w => w.WarrantyHistories)
                .Include(x => x.WarrantyRequestItems).ThenInclude(x => x.OrderItem).ThenInclude(x => x.Order)
                .ThenInclude(x => x.User).ThenInclude(x => x.Role)
                .Where(x => !x.IsDeleted && x.WarrantyRequestItems.Any(i => i.DestinationBranchId == branchId));

            return GetPaging(query, index, pageSize);
        }
    }
}