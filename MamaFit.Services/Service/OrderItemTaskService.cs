using AutoMapper;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Entity;
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

    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByAssignedStaffAsync(string accessToken)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var orderItemTasks = await _repo.GetTasksByAssignedStaffAsync(userId);
        _validation.CheckNotFound(orderItemTasks, "No tasks found for the assigned staff");

        //Lấy tasks của milestone
        var allTasksWithMilestones = orderItemTasks
            .Where(x => x.MaternityDressTask != null && x.MaternityDressTask.Milestone != null)
            .Select(x => new
            {
                OrderItemTask = x,
                Task = x.MaternityDressTask,
                Milestone = x.MaternityDressTask.Milestone
            })
            .ToList();

        //Nhóm theo group id
        var groupedByMilestone = allTasksWithMilestones
            .GroupBy(x => x.Milestone.Id)
            .Select(group =>
            {
                var representative = group.First();

                return new OrderItemTaskGetByTokenResponse
                {
                    Id = representative.OrderItemTask.OrderItem.Id,
                    CreatedAt = representative.OrderItemTask.CreatedAt,
                    UpdatedAt = (DateTime)representative.OrderItemTask.UpdatedAt,
                    CreatedBy = representative.OrderItemTask.CreatedBy,
                    UpdatedBy = representative.OrderItemTask.UpdatedBy,
                    Preset = _mapper.Map<PresetGetAllResponseDto>(representative.OrderItemTask.OrderItem.Preset),
                    DesignRequest = _mapper.Map<DesignResponseDto>(representative.OrderItemTask.OrderItem.DesignRequest),
                    MaternityDressDetail = _mapper.Map<MaternityDressDetailResponseDto>(representative.OrderItemTask.OrderItem.MaternityDressDetail),

                    Milestones = new MilestoneGetByIdOrderTaskResponseDto
                    {
                        Name = representative.Milestone.Name,
                        Description = representative.Milestone.Description,
                        ApplyFor = representative.Milestone.ApplyFor,
                        SequenceOrder = representative.Milestone.SequenceOrder,

                        MaternityDressTasks = group
                            .Select(g => _mapper.Map<MaternityDressTaskOrderTaskResponseDto>(g.Task))
                            .ToList()
                    }
                };
            })
            .ToList();

        return groupedByMilestone;
    }

    public async Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskStatus status)
    {
        var task = await _repo.GetByIdAsync(dressTaskId, orderItemId);
        _validation.CheckNotFound(task, "Order item task not found");
        
        await _repo.UpdateOrderItemTaskStatusAsync(task, status);
        await _repo.UpdateAsync(task);
    }
}