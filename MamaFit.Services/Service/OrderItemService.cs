using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidationService _validation;
    public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _httpContextAccessor = httpContextAccessor;
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

        var milestone = await _unitOfWork.MilestoneRepository.GetByIdDetailAsync(request.MilestoneId!);
        _validation.CheckNotFound(milestone, $"Milestone with id:{request.MilestoneId} is not exist!");

        foreach (var task in milestone.MaternityDressTasks!)
        {
            orderItemTasks.Add(new OrderItemTask
            {
                MaternityDressTask = task,
                MaternityDressTaskId = task.Id,
                User = user?? null,
                UserId = user?.Id ?? string.Empty,
                OrderItem = orderItem,
                OrderItemId = orderItem.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.UserName ?? "System",
            });
        }
        orderItem.OrderItemTasks = orderItemTasks;

        await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem);
        await _unitOfWork.SaveChangesAsync();
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value ?? string.Empty;
    }
}