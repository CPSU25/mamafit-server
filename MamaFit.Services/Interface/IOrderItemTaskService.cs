using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;

namespace MamaFit.Services.Interface;

public interface IOrderItemTaskService
{
    Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByAssignedStaffAsync();
    Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByOrderItemIdAsync(string orderItemId);
    Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskUpdateRequestDto request, bool? severity);
}