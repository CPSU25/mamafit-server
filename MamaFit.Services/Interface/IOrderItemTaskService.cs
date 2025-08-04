using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Services.Interface;

public interface IOrderItemTaskService
{
    Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByAssignedStaffAsync();
    Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByOrderItemIdAsync(string orderItemId);
    Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskUpdateRequestDto request, bool? severity);
}