﻿using MamaFit.BusinessObjects.DBContext;
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
            var orderItemTask = await _context.OrderItemsTasks.
                Include(x => x.User).
                Include(x => x.MaternityDressTask).
                Include(x => x.OrderItem).
                FirstOrDefaultAsync(x => x.OrderItemId.Equals(request.OrderItemId)
                && x.MaternityDressTaskId.Equals(request.MaternityDressTaskId));

            return orderItemTask;
        }

        public async Task UpdateOrderItemTaskStatusAsync(OrderItemTask task, OrderItemTaskStatus status)
        {
            task.Status = status;
            task.UpdatedBy = GetCurrentUserName();
            task.UpdatedAt = DateTime.UtcNow;

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            var username = _httpContextAccessor.HttpContext?.User?.FindFirst("userName")?.Value;
            return username ?? "System";
        }
    }
}
