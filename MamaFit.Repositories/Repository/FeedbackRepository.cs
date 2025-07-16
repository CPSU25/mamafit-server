using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

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
}