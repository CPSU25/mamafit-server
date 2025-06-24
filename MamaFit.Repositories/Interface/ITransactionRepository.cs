using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<PaginatedList<Transaction>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<Transaction?> GetByOrderIdAsync(string id);
    Task<Order?> GetOrderByPaymentCodeAsync(string paymentCode);
}