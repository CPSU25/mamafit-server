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

    public async Task<List<Order>> GetOrdersByDesignerAsync(string designerId)
    {
        return await _context.Orders
            .Where(o => o.OrderItems.Any(oi =>
                oi.ItemType == ItemType.DESIGN_REQUEST &&
                oi.DesignRequest != null &&
                oi.DesignRequest.UserId == designerId))
            .Include(o => o.OrderItems.Where(oi =>
                oi.ItemType == ItemType.DESIGN_REQUEST &&
                oi.DesignRequest != null &&
                oi.DesignRequest.UserId == designerId))
            .ThenInclude(oi => oi.DesignRequest)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrderForRequest(string userId)
    {
        var response = _dbSet.Include(x => x.OrderItems).ThenInclude(x => x.Preset)
            .Where(x => !x.IsDeleted
            && x.Status == OrderStatus.COMPLETED
            && x.UserId == userId
            && x.OrderItems.Any(x => (x.ItemType == ItemType.PRESET || x.ItemType == ItemType.WARRANTY) && x.WarrantyDate == null));

        return await response.ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByBranchManagerAsync(string managerId)
    {
        var branchIds = await _context.Branches
            .Where(b => b.BranchManagerId == managerId)
            .Select(b => b.Id)
            .ToListAsync();

        return await _context.Orders
            .Where(o => branchIds.Contains(o.BranchId!))
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.DesignRequest)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByAssignedStaffAsync(string staffId)
    {
        return await _context.Orders
            .Where(o => o.OrderItems.Any(oi =>
                oi.OrderItemTasks.Any(task => task.UserId == staffId)))
            .Include(o => o.OrderItems
                .Where(oi => oi.OrderItemTasks.Any(task => task.UserId == staffId)))
            .ThenInclude(oi => oi.OrderItemTasks)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.DesignRequest)
            .ToListAsync();
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
            .Include(x => x.Measurement).ThenInclude(x => x.MeasurementDiary)
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

        query = query.OrderByDescending(x => x.CreatedAt);
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
            .Include(x => x.Measurement)
            .ThenInclude(x => x.MeasurementDiary)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.MaternityDressDetail)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Preset)
                    .ThenInclude(x => x.Style)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.DesignRequest)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Role)
            .Include(x => x.Branch)
            .Include(x => x.Address)
            .Include(x => x.VoucherDiscount)
            .Include(x => x.OrderItems).ThenInclude(x => x.OrderItemAddOnOptions).ThenInclude(x => x.AddOnOption).ThenInclude(x => x.Size)
            .Include(x => x.OrderItems).ThenInclude(x => x.OrderItemAddOnOptions).ThenInclude(x => x.AddOnOption).ThenInclude(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Order?> GetWithItemsAndDressDetails(string id)
    {
        return await _dbSet
            .Include(x => x.Branch)
            .Include(x => x.Address)
            .Include(x => x.User)
            .Include(x => x.OrderItems).ThenInclude(x => x.ParentOrderItem).ThenInclude(x => x.Order)
            .Include(x => x.OrderItems.Where(oi => !oi.IsDeleted))
            .ThenInclude(oi => oi.MaternityDressDetail)
            .Include(x => x.OrderItems.Where(oi => !oi.IsDeleted))
            .ThenInclude(oi => oi.Preset)
            .Include(x => x.OrderItems).ThenInclude(x => x.WarrantyRequestItems).ThenInclude(x => x.WarrantyRequest)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Order?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .Include(x => x.OrderItems).ThenInclude(x => x.OrderItemTasks).ThenInclude(x => x.MaternityDressTask).ThenInclude(x => x.Milestone)
            .Include(x => x.OrderItems).ThenInclude(x => x.OrderItemAddOnOptions).ThenInclude(x => x.AddOnOption).ThenInclude(x => x.AddOn)
            .FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
    }
}