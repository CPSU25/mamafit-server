using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Repositories.Interface
{
    public interface IOrderItemTaskRepository
    {
        Task<List<OrderItemTask>> GetTasksByAssignedStaffAsync(string staffId);
        Task<OrderItemTask> GetDetailAsync(OrderItemTaskGetDetail request);
        Task UpdateOrderItemTaskStatusAsync(OrderItemTask task, OrderItemTaskStatus status);
        Task UpdateAsync(OrderItemTask task);
    }
}
