using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Services.Interface;

public interface IOrderItemTaskService
{
    Task<StaffTasksGroupedResponse> GetTasksByAssignedStaffAsync(string accessToken);
    Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskStatus status);
}