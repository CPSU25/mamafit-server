using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<PaginatedList<Order>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<Order?> GetByIdWithItems(string id);
    Task<Order?> GetByCodeAsync(string code);
    Task<Order?> GetWithItemsAndDressDetails(string id);
}