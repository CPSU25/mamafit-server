using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }

    public async Task<PaginatedList<Feedback>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
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
        return await PaginatedList<Feedback>.CreateAsync(query, index, pageSize);
    }

    public async Task<List<Feedback>> GetAllByUserId(string userId)
    {
        var query = await _dbSet
            .Include(x => x.OrderItem)
            .ThenInclude(x => x.Order)
            .Where(x => !x.IsDeleted && x.UserId == userId).ToListAsync();
        return query;
    }

    public async Task<Feedback> GetFeedbackByUserIdAndOrderItemId(string userId, string orderItemId)
    {
        var result = await _dbSet.FirstOrDefaultAsync(x => !x.IsDeleted && x.UserId == userId && x.OrderItemId == orderItemId);

        return result;
    }
}