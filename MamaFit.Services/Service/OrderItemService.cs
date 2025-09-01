using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.OrderItemTaskDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidationService _validation;
    private readonly IConfigService _configService;
    private readonly ICacheService _cacheService;
    private readonly IOrderItemTaskService _orderItemTaskService;
    private readonly IMilestoneService _milestoneService;

    public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, IHttpContextAccessor httpContextAccessor, IConfigService configService, ICacheService cacheService, IOrderItemTaskService orderItemTaskService, IMilestoneService milestoneService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _httpContextAccessor = httpContextAccessor;
        _configService = configService;
        _cacheService = cacheService;
        _orderItemTaskService = orderItemTaskService;
        _milestoneService = milestoneService;
    }

    public async Task<PaginatedList<OrderItemResponseDto>> GetAllOrderItemsAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var orderItems = await _unitOfWork.OrderItemRepository.GetAllAsync(index, pageSize, startDate, endDate);

        var responseItems = orderItems.Items
            .Select(orderItem => _mapper.Map<OrderItemResponseDto>(orderItem))
            .ToList();

        var responsePaginatedList = new PaginatedList<OrderItemResponseDto>(
            responseItems,
            orderItems.TotalCount,
            orderItems.PageNumber,
            pageSize
        );

        return responsePaginatedList;
    }

    public async Task<OrderItemGetByIdResponseDto> GetOrderItemByIdAsync(string id)
    {
        var orderItem = await _unitOfWork.OrderItemRepository.GetDetailById(id);
        _validation.CheckNotFound(orderItem, "Order item is not exist!");
        return _mapper.Map<OrderItemGetByIdResponseDto>(orderItem);
    }

    public async Task<OrderItemResponseDto> CreateOrderItemAsync(OrderItemRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);

        var config = await _configService.GetConfig();

        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(model.OrderId);
        _validation.CheckNotFound(order, "Order is not exist!");

        var maternityDressDetail = await _unitOfWork.MaternityDressDetailRepository.GetByIdNotDeletedAsync(model.MaternityDressDetailId);
        _validation.CheckNotFound(maternityDressDetail, "Maternity dress detail is not exist!");

        var orderItem = _mapper.Map<OrderItem>(model);
        await _unitOfWork.OrderItemRepository.InsertAsync(orderItem);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderItemResponseDto>(orderItem);
    }

    public async Task<OrderItemResponseDto> UpdateOrderItemAsync(string id, OrderItemRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var orderItem = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(orderItem, "Order item is not exist!");

        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(model.OrderId);
        _validation.CheckNotFound(order, "Order is not exist!");

        var maternityDressDetail = await _unitOfWork.MaternityDressDetailRepository.GetByIdNotDeletedAsync(model.MaternityDressDetailId);
        _validation.CheckNotFound(maternityDressDetail, "Maternity dress detail is not exist!");

        orderItem = _mapper.Map(model, orderItem);
        await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OrderItemResponseDto>(orderItem);
    }

    public async Task DeleteOrderItemAsync(string id)
    {
        var orderItem = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(orderItem, "Order item is not exist!");
        orderItem.IsDeleted = true;
        await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignTaskToOrderItemAsync(AssignTaskToOrderItemRequestDto request)
    {
        var currentUserId = GetCurrentUserId() ?? null;
        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(currentUserId);

        var orderItem = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(request.OrderItemId!);
        _validation.CheckNotFound(orderItem, $"Order item with id:{request.OrderItemId} is not exist!");

        var orderItemTasks = new List<OrderItemTask>();

        foreach (var milestoneId in request.MilestoneIds!)
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdDetailAsync(milestoneId!);
            _validation.CheckNotFound(milestone, $"Milestone with id:{milestoneId} is not exist!");

            foreach (var task in milestone.MaternityDressTasks!)
            {
                orderItemTasks.Add(new OrderItemTask
                {
                    MaternityDressTask = task,
                    MaternityDressTaskId = task.Id,
                    OrderItem = orderItem,
                    OrderItemId = orderItem.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user?.UserName ?? "System",
                });
            }
        }

        orderItem.OrderItemTasks = orderItemTasks;

        await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem);
        await _unitOfWork.SaveChangesAsync();
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value ?? string.Empty;
    }

    public async Task AssignChargeToOrderItemAsync(AssignChargeToOrderItemRequestDto request, ApplicationUser user)
    {
        var milestone = await _unitOfWork.MilestoneRepository.GetByIdDetailAsync(request.MilestoneId!);
        _validation.CheckNotFound(milestone, $"Milestone with id:{request.MilestoneId} is not exist!");

        var personInCharge = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(request.ChargeId!);
        _validation.CheckNotFound(personInCharge, $"Person in charge with id:{request.ChargeId} is not exist!");

        foreach (var orderItemId in request.OrderItemIds!)
        {
            var orderItem = await _unitOfWork.OrderItemRepository.GetDetailById(orderItemId!);
            _validation.CheckNotFound(orderItem, $"Order item with id:{orderItemId} is not exist!");

            foreach (var task in milestone.MaternityDressTasks!)
            {
                var orderItemTask = orderItem!.OrderItemTasks!
                    .FirstOrDefault(x => x.MaternityDressTask != null && x.MaternityDressTask.Equals(task));

                if (orderItemTask != null)
                {
                    orderItemTask.User = personInCharge;
                    orderItemTask.UserId = personInCharge!.Id;
                    orderItemTask.UpdatedBy = user?.UserName ?? "System";
                    orderItemTask.Deadline = DateTime.UtcNow.AddMinutes(task.EstimateTimeSpan * task.SequenceOrder);
                    orderItemTask.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.OrderItemTaskRepository.UpdateAsync(orderItemTask);
                }
            }
            await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem!);
        }
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignChargeToOrderItemListAsync(List<AssignChargeToOrderItemRequestDto> requests)
    {
        var currentUserId = GetCurrentUserId() ?? null;
        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(currentUserId);

        foreach (var request in requests)
        {
            await AssignChargeToOrderItemAsync(request, user);
        }
    }

    public async Task CheckListStatusForOrderItemTaskAsync(OrderItemCheckTaskRequestDto request)
    {
        var cacheKey = "MilestoneAchiveOrderItemResponseDto";

        foreach (var task in request.MaternityDressTaskIds!)
        {
            var orderItemTask = await _unitOfWork.OrderItemTaskRepository.GetDetailAsync(new OrderItemTaskGetDetail
            {
                OrderItemId = request.OrderItemId,
                MaternityDressTaskId = task
            });

            _validation.CheckNotFound(orderItemTask, $"Order item task with order item id:{request.OrderItemId} and maternity dress task id:{task} is not exist!");

            if (orderItemTask.User == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, $"Order item task with order item id:{request.OrderItemId} and maternity dress task id:{task} is do not have a charger!!");

            await _orderItemTaskService.UpdateStatusAsync(task, request.OrderItemId, new OrderItemTaskUpdateRequestDto
            {
                Status = request.Status,
            }, request.Severity);
        }

        await _cacheService.RemoveByPrefixAsync(cacheKey);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<DesignerInfoOrderItemResponseDto> GetDesignerInfoByOrderItemIdAsync(string orderItemId)
    {
        var currentUserId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(currentUserId);
        _validation.CheckNotFound(user, "Current user is not exist!");

        var orderItem = await _unitOfWork.OrderItemRepository.GetDetailById(orderItemId);
        _validation.CheckNotFound(orderItem, $"Order item with id:{orderItemId} is not exist!");

        var designerId = orderItem.OrderItemTasks
            .Where(x => x.MaternityDressTask.Milestone.ApplyFor.Contains(ItemType.DESIGN_REQUEST))
            .Select(x => x.UserId)
            .FirstOrDefault();

        var designerInfo = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(designerId);

        var chatRoom = await _unitOfWork.ChatRepository.GetRoomBetweenUsersAsync(designerInfo?.Id, currentUserId) ?? null;

        var response = new DesignerInfoOrderItemResponseDto
        {
            ChatRoomId = chatRoom?.Id,
            Designer = _mapper.Map<UserReponseDto>(designerInfo) ?? null
        };

        return response;
    }

    public async Task<SequenceOrderResponseDto> GetCurrentOrderItemTaskSequence(string orderItemId)
    {
        var progressList = await _milestoneService.GetMilestoneByOrderItemId(orderItemId);
        foreach (var progress in progressList)
        {
            if (progress.Progress >= 100)
                continue;
            else
            {
                var maternityTask = await _unitOfWork.MaternityDressTaskRepository.GetByIdAsync(progress.CurrentTask.Id);
                return new SequenceOrderResponseDto
                {
                    Milestone = progress.Milestone!.SequenceOrder,
                    Task = maternityTask.SequenceOrder,
                };
            }
        }
        return new SequenceOrderResponseDto
        {
        };
    }
}