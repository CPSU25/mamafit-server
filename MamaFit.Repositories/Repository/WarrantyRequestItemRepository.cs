using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class WarrantyRequestItemRepository : IWarrantyRequestItemRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly DbSet<WarrantyRequestItem> _dbSet;
    public WarrantyRequestItemRepository(
        ApplicationDbContext context, IOrderItemRepository orderItemRepository)
    {
        _context = context;
        _dbSet = _context.Set<WarrantyRequestItem>();
        _orderItemRepository = orderItemRepository;
    }

    public async Task<WarrantyRequestItem?> GetByIdAsync(string itemId, string requestId)
    {
        if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(requestId))
            return null;

        return await _dbSet
            .Include(x => x.WarrantyRequest)
            .Include(x => x.OrderItem)
            .FirstOrDefaultAsync(x => x.OrderItemId == itemId && x.WarrantyRequestId == requestId);
    }

    public async Task<PaginatedList<WarrantyRequestItem>> GetAllAsync(int index, int pageSize, string? search)
    {
        var query = _dbSet
            .Include(x => x.OrderItem)
            .ThenInclude(x => x.Order)
            .Include(x => x.DestinationBranch)
            .Include(x => x.WarrantyRequest)
            .Include(x => x.OrderItem.Preset)
            .Include(x => x.OrderItem.MaternityDressDetail)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(x => x.WarrantyRequest.SKU.Contains(search));

        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task InsertAsync(WarrantyRequestItem entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountWarrantyRequestItemsAsync(string orderItemId)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
        if (orderItem == null) return 0;

        var rootId = orderItem.ParentOrderItemId ?? orderItem.Id;
        var all = await _orderItemRepository.GetAllAsync();

        var count = all.Count(oi => oi.ParentOrderItemId == rootId && oi.ItemType == ItemType.WARRANTY);
        return count;
    }


    public async Task<WarrantyRequestItem> GetByOrderItemIdAsync(string orderItemId)
    {
        return await _dbSet.AsNoTracking()
        .Include(wri => wri.WarrantyRequest)
        .Include(wri => wri.OrderItem)
            .ThenInclude(oi => oi.ParentOrderItem)
                .ThenInclude(poi => poi.Preset).ThenInclude(x => x.Style)
        .Include(x => x.OrderItem).ThenInclude(x => x.ParentOrderItem).ThenInclude(x => x.Order)
        .FirstOrDefaultAsync(wri => wri.OrderItemId == orderItemId);
    }

    public async Task<List<WarrantyRequestItem>> GetAllRelatedByOrderItemAsync(string orderItemId)
    {
        var result = await _dbSet.AsNoTracking()
            .Include(wri => wri.WarrantyRequest)
            .Include(wri => wri.OrderItem)
            .ThenInclude(oi => oi.ParentOrderItem)
                .ThenInclude(poi => poi.Preset).ThenInclude(x => x.Style)
        .Include(x => x.OrderItem).ThenInclude(x => x.ParentOrderItem).ThenInclude(x => x.Order)
        .Where(x => x.OrderItem.Id == orderItemId || x.OrderItem.ParentOrderItemId == orderItemId).ToListAsync();

        return result;
    }

    public async Task UpdateAsync(WarrantyRequestItem entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}