using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMilestoneService _milestoneService;

    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IMilestoneService milestoneService)
    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
        _unitOfWork = unitOfWork;
        _milestoneService = milestoneService;
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
                AddressId = representative.OrderItem.Order.AddressId,
                MeasurementDiary = _mapper.Map<MeasurementDiaryResponseDto>(representative.OrderItem.Order.MeasurementDiary),
                OrderCode = representative.OrderItem.Order.Code,
                Milestones = milestoneGroups
            };
        })
        .ToList();

        return groupedByOrderItem;
    }

    public async Task<List<OrderItemTaskGetByTokenResponse>> GetTasksByOrderItemIdAsync(string orderItemId)
    {
        var listOrderItemTask = await GetTasksByAssignedStaffAsync();

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

        if (request.Status == OrderItemTaskStatus.IN_PROGRESS)
        {
            if (task.MaternityDressTask!.Milestone!.ApplyFor!.Contains(ItemType.DESIGN_REQUEST))
            {
                task.OrderItem!.Order!.Status = OrderStatus.IN_DESIGN;

                await _repo.UpdateAsync(task);
                return;
            }
            task.OrderItem!.Order!.Status = OrderStatus.IN_PRODUCTION;
        }

        if (request.Status == OrderItemTaskStatus.DONE)
        {
            var applyFor = task.MaternityDressTask!.Milestone!.ApplyFor!;
            bool hasPreset = applyFor.Contains(ItemType.PRESET);
            bool hasAddon = applyFor.Contains(ItemType.ADD_ON);

            if (hasPreset)
            {
                var progressList = await _milestoneService.GetMilestoneByOrderItemId(orderItemId);

                bool isCurrentMilestoneDone = progressList.Any(x =>
                    x.Milestone.Id == task.MaternityDressTask.MilestoneId &&
                    x.IsDone &&
                    x.Progress == 100
                );

                if (!isCurrentMilestoneDone) return;

                if (hasAddon)
                {
                    // Tìm milestone thuộc loại ADD_ON khác milestone hiện tại
                    var addonMilestoneDone = progressList.Any(x =>
                        x.Milestone.Id != task.MaternityDressTask.MilestoneId &&
                        x.Milestone.ApplyFor.Contains(ItemType.ADD_ON) &&
                        x.IsDone &&
                        x.Progress == 100
                    );

                    if (!addonMilestoneDone) return;
                }

                // Cả milestone hiện tại (PRESET) và milestone ADD_ON (nếu có) đều đạt yêu cầu
                task.OrderItem!.Order!.Status = OrderStatus.IN_QC;
                await _repo.UpdateAsync(task);
                return;
            }
        }
        else if (task.MaternityDressTask!.Milestone.ApplyFor!.Contains(ItemType.DESIGN_REQUEST))
        {
            task.OrderItem!.Order!.Status = OrderStatus.COMPLETED;
            await _repo.UpdateAsync(task);
            return;
        }
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirst("userId").Value ?? "System";
    }
}