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
using MamaFit.Services.ExternalService.Redis;
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
    private readonly ICacheService _cacheService;

    public OrderItemTaskService(IOrderItemTaskRepository repo, IMapper mapper, IValidationService validation, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IMilestoneService milestoneService, ICacheService cacheService)
    {
        _repo = repo;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
        _unitOfWork = unitOfWork;
        _milestoneService = milestoneService;
        _cacheService = cacheService;
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
                Measurement = _mapper.Map<MeasurementResponseDto>(representative.OrderItem.Order.Measurement),
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
        var cacheKey = "MilestoneAchiveOrderItemResponseDto";
        var task = await _repo.GetByIdAsync(dressTaskId, orderItemId);
        _validation.CheckNotFound(task, "Order item task not found");

        task.Note = request.Note;
        task.Status = request.Status;
        task.Image = request.Image;

        await _repo.UpdateAsync(task);
        await _cacheService.RemoveByPrefixAsync(cacheKey);

        var orderItem = await _unitOfWork.OrderItemRepository.GetDetailById(orderItemId);
        var order = task.OrderItem.Order;
        if (request.Status == OrderItemTaskStatus.IN_PROGRESS)
        {
            if(orderItem.ItemType == ItemType.DESIGN_REQUEST)
            {
                task.OrderItem.Order.Status = OrderStatus.IN_DESIGN;
            }
            if(orderItem.ItemType == ItemType.PRESET)
            {
                task.OrderItem.Order.Status = OrderStatus.IN_PRODUCTION;
            }
            await _unitOfWork.OrderRepository.UpdateAsync(task.OrderItem.Order);
            await _unitOfWork.SaveChangesAsync();
            return;
        }
        if(request.Status == OrderItemTaskStatus.DONE)
        {
            if(orderItem.ItemType == ItemType.DESIGN_REQUEST)
            {
                order.Status = OrderStatus.COMPLETED;
            }
            if(orderItem.ItemType== ItemType.PRESET)
            {
                var progress = await _milestoneService.GetMilestoneByOrderItemId(orderItemId);
                var presetProgress = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.PRESET));
                if(presetProgress.Any(x => x.Progress == 100 && x.IsDone))
                {
                    var hasAddOn = progress.Where(x => x.Milestone.ApplyFor.Contains(ItemType.ADD_ON));
                    if (hasAddOn.Any())
                    {
                        if(hasAddOn.Any(x => x.Progress == 100 && x.IsDone)){
                            order.Status = OrderStatus.IN_QC;
                        }
                    }
                    else
                    {
                        order.Status = OrderStatus.IN_QC;
                    }
                }
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();
                return;
            }
        }
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirst("userId").Value ?? "System";
    }
}