using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class OrderItemTaskRepository : IOrderItemTaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderItemTaskRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderItemTask> GetDetailAsync(OrderItemTaskGetDetail request)
        {
            var orderItemTask = await _context.OrderItemsTasks.AsNoTracking().
                Include(x => x.User).
                Include(x => x.MaternityDressTask).
                Include(x => x.OrderItem).
                FirstOrDefaultAsync(x => x.OrderItemId.Equals(request.OrderItemId)
                && x.MaternityDressTaskId.Equals(request.MaternityDressTaskId));
            return orderItemTask;
        }

        public async Task<OrderItemTask> GetByIdAsync(string maternityDressTaskId, string orderItemId)
        {
            var orderItemTask = await _context.OrderItemsTasks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaternityDressTaskId == maternityDressTaskId && x.OrderItemId == orderItemId);
            return orderItemTask;
        }

        public async Task UpdateAsync(OrderItemTask task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderItemTaskStatusAsync(OrderItemTask task, OrderItemTaskStatus status)
        {
            task.Status = status;
            task.UpdatedBy = GetCurrentUserName();
            task.UpdatedAt = DateTime.UtcNow;

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderItemTask>> GetTasksByAssignedStaffAsync(string userId)
        {
            var result = await _context.OrderItemsTasks
                .Where(t => t.UserId == userId)
                .Include(t => t.MaternityDressTask).ThenInclude(t => t!.Milestone)
                .Include(t => t.OrderItem).ThenInclude(o => o!.Preset).ThenInclude(x => x.Style)
                .Include(t => t.OrderItem).ThenInclude(o => o!.DesignRequest)
                .Include(t => t.OrderItem).ThenInclude(o => o!.MaternityDressDetail)
                .ToListAsync();

            return result;
        }


        private string GetCurrentUserName()
        {
            var username = _httpContextAccessor.HttpContext?.User?.FindFirst("userName")?.Value;
            return username ?? "System";
        }
    }
}
