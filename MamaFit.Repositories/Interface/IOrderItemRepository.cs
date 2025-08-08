using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<PaginatedList<OrderItem>> GetAllAsync(int index, int pageSize, DateTime? startDate,
            DateTime? endDate);
        Task<OrderItem> GetDetailById(string orderItemId);
        Task<List<OrderItem>> GetOrderItemByUserId(string userId);
    }
}
