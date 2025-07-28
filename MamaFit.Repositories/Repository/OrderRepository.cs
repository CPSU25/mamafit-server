using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
    {
    }

    public async Task<PaginatedList<Order>> GetByTokenAsync(int index, int pageSize, string token, string? search, OrderStatus? status = null)
    {
        var query = _dbSet
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.MaternityDressDetail)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Preset)
            .ThenInclude(x => x.Style)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.DesignRequest)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.UserId == token);
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.Code.Contains(search));
        }
        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<PaginatedList<Order>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);
        if (startDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= endDate.Value);
        }

        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<Order?> GetByIdWithItems(string id)
    {
        return await _dbSet.AsNoTracking()
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.MaternityDressDetail)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Preset)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.DesignRequest)
            .ThenInclude(x => x.User)
            .Include(x => x.Branch)
            .Include(x => x.Address)
            .Include(x => x.VoucherDiscount)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Order?> GetWithItemsAndDressDetails(string id)
    {
        return await _dbSet.AsNoTracking()
            .Include(x => x.OrderItems.Where(oi => !oi.IsDeleted))
            .ThenInclude(oi => oi.MaternityDressDetail)
            .Include(x => x.OrderItems.Where(oi => !oi.IsDeleted))
            .ThenInclude(oi => oi.Preset)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Order?> GetByCodeAsync(string code)
    {
        return await _dbSet.AsNoTracking()
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
    }
}