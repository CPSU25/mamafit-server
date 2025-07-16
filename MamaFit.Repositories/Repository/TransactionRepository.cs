using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }

    public async Task<PaginatedList<Transaction>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var transactions = _dbSet
            .Where(x => !x.IsDeleted);
        if (startDate.HasValue)
            transactions = transactions.Where(x => x.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            transactions = transactions.Where(x => x.CreatedAt <= endDate.Value);
        return await transactions.GetPaginatedList(index, pageSize);
    }
    
    public async Task<Transaction?> GetByOrderIdAsync(string orderId) 
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.OrderId == orderId && !x.IsDeleted);
    }
    
    public async Task<Order?> GetOrderByPaymentCodeAsync(string paymentCode)
    {
        return await _dbSet
            .Include(t => t.Order)
            .Where(t => t.Code == paymentCode && !t.IsDeleted)
            .Select(t => t.Order)
            .FirstOrDefaultAsync();
    }
}