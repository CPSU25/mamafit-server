using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class OrderItemTaskService : IOrderItemTaskService
{
    private readonly IOrderItemTaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _contextAccessor;

    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation, IHttpContextAccessor contextAccessor)
    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
    }

    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByAssignedStaffAsync()
    {
        var userId = GetCurrentUserId();
        var orderItemTasks = await _repo.GetTasksByAssignedStaffAsync(userId);
        _validation.CheckNotFound(orderItemTasks, "No tasks found for the assigned staff");

        var groupedByOrderItem = orderItemTasks
        .Where(x => x.OrderItem != null)
        .GroupBy(x => x.OrderItem.Id)
        .Select(orderGroup =>
        {
            var representative = orderGroup.First();

            var milestoneGroups = orderGroup
                .Where(x => x.MaternityDressTask != null && x.MaternityDressTask.Milestone != null)
                .GroupBy(x => x.MaternityDressTask.Milestone.Id)
                .Select(milestoneGroup =>
                {
                    var milestoneRep = milestoneGroup.First().MaternityDressTask.Milestone;

                    return new MilestoneGetByIdOrderTaskResponseDto
                    {
                        Name = milestoneRep.Name,
                        Description = milestoneRep.Description,
                        ApplyFor = milestoneRep.ApplyFor,
                        SequenceOrder = milestoneRep.SequenceOrder,
                        MaternityDressTasks = milestoneGroup
                            .Select(x => _mapper.Map<MaternityDressTaskOrderTaskResponseDto>(x.MaternityDressTask))
                            .ToList()
                    };
                })
                .ToList();

            return new OrderItemTaskGetByTokenResponse
            {
                OrderItem = _mapper.Map<OrderItemResponseDto>(representative.OrderItem),
                Milestones = milestoneGroups
            };
        })
        .ToList();

        return groupedByOrderItem;
    }

    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByOrderItemIdAsync(string orderItemId)
    {
        var listOrderItemTask = await  GetTasksByAssignedStaffAsync();

        var response = listOrderItemTask.Where(x => x.OrderItem.Id == orderItemId).ToList();
        _validation.CheckNotFound(response, $"OrderItemTask with OrderItemId: {orderItemId} is not found");
        return response;
    }

    public async Task UpdateStatusAsync(string dressTaskId, string orderItemId, OrderItemTaskUpdateRequestDto request)
    {
        var task = await _repo.GetByIdAsync(dressTaskId, orderItemId);
        _validation.CheckNotFound(task, "Order item task not found");

        task.Note = request.Note;
        task.Status = request.Status;
        task.Image = request.Image;

        await _repo.UpdateAsync(task);
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirst("userId").Value ?? "System";
    }
}