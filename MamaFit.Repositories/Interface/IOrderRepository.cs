using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<List<Order>> GetOrdersByDesignerAsync(string designerId);
    Task<Order> GetBySkuAndCodeAsync(string sku, string code);
    Task<List<Order>> GetOrdersByBranchManagerAsync(string managerId);
    Task<List<Order>> GetOrdersByAssignedStaffAsync(string staffId);
    Task<PaginatedList<Order>> GetByTokenAsync(int index, int pageSize, string token, string? search, OrderStatus? status = null);
    Task<PaginatedList<Order>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<List<Order>> GetOrderForRequest(string userId);
    Task<List<Order>> GetAllOrderByDesignRequestId(string designRequestId);
    Task<Order?> GetByIdWithItems(string id);
    Task<Order?> GetByCodeAsync(string code);
    Task<Order?> GetWithItemsAndDressDetails(string id);
    Task<Order> GetByOrderItemId(string orderItemId);
}