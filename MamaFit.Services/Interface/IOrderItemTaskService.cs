using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;

namespace MamaFit.Services.Interface;

public interface IOrderItemTaskService
{
    Task<List<AssignStaffDto>> GetTasksByAssignedStaffAsync(string staffId);
}