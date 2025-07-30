using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class OrderItemTaskService : IOrderItemTaskService
{
    private readonly IOrderItemTaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation)
    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<StaffTasksGroupedResponse> GetTasksByAssignedStaffAsync(string accessToken)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var tasks = await _repo.GetTasksByAssignedStaffAsync(userId);

        // Map và gom theo OrderItem
        var grouped = tasks
            .GroupBy(t => t.OrderItemId)
            .Select(group => new StaffOrderItemTaskGroupDto
            {
                OrderItemId = group.Key,
                OrderItem = _mapper.Map<OrderItemResponseDto>(group.First().OrderItem!),
                Tasks = group.Select(task =>
                {
                    var dto = _mapper.Map<StaffTaskDetailDto>(task);
                    dto.MilestoneId = task.MaternityDressTask?.Milestone?.Id;
                    return dto;
                }).ToList()
            })
            .ToList();

        // Lấy unique milestone
        var milestones = tasks
            .Select(t => t.MaternityDressTask?.Milestone)
            .Where(m => m != null)
            .GroupBy(m => m!.Id)
            .Select(g => _mapper.Map<MilestoneResponseMinDto>(g.First()!))
            .ToList();

        return new StaffTasksGroupedResponse
        {
            Data = grouped,
            Milestones = milestones
        };
    }
    
    public async Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskStatus status)
    {
        var task = await _repo.GetByIdAsync(dressTaskId, orderItemId);
        _validation.CheckNotFound(task, "Order item task not found");
        
        await _repo.UpdateOrderItemTaskStatusAsync(task, status);
        await _repo.UpdateAsync(task);
    }
}