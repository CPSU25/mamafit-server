using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<OrderItem>> GetAllAsync(int index, int pageSize, DateTime? startDate,
            DateTime? endDate)
        {
            var query = _dbSet.AsNoTracking().Where(x => !x.IsDeleted);
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

        public async Task<OrderItem> GetDetailById(string orderItemId)
        {
            var result = await _dbSet
                .Include(x => x.DesignRequest)
                .Include(x => x.MaternityDressDetail)
                .Include(x => x.Preset)
                .Include(x => x.OrderItemTasks)!
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Role)
                .Include(x => x.OrderItemTasks)
                    .ThenInclude(x => x.MaternityDressTask)
                        .ThenInclude(x => x!.Milestone)
                .FirstOrDefaultAsync(x => x.Id == orderItemId && !x.IsDeleted);
            return result!;
        }

        public async Task<List<OrderItem>> GetOrderItemByUserId(string userId)
        {
            var result = await _dbSet
                .Include(x => x.DesignRequest)
                .Include(x => x.MaternityDressDetail)
                .Include(x => x.Preset)
                .Include(x => x.OrderItemTasks)!
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Role)
                .Include(x => x.OrderItemTasks)
                    .ThenInclude(x => x.MaternityDressTask)
                        .ThenInclude(x => x!.Milestone)
                .Where(x => !x.IsDeleted && x.OrderItemTasks.Select(x => x.UserId == userId).Any()).ToListAsync();

            return result;
        }
    }
}
