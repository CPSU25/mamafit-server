using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
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
        var query = _dbSet.AsNoTracking()
            .Include(x => x.WarrantyRequest)
            .Include(x => x.OrderItem)
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
    
    public async Task<int> CountWarrantyRequestItemsAsync(string requestId)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(requestId);
        if (orderItem.ParentOrderItemId == null)
            return 1;
        else
        {
            var orderItemList = await _orderItemRepository.GetAllAsync();
            var result = orderItemList.Count( x => x.ParentOrderItemId == orderItem.ParentOrderItemId);
            return result;
        }
    }
}