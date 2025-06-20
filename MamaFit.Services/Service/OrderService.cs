using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<PaginatedList<OrderResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var orders = await _unitOfWork.OrderRepository.GetAllAsync(index, pageSize, startDate, endDate);
        var responseItems = orders.Items
            .Select(order => _mapper.Map<OrderResponseDto>(order))
            .ToList();
        return new PaginatedList<OrderResponseDto>(
            responseItems,
            orders.TotalCount,
            orders.PageNumber,
            pageSize
        );
    }
    
    public async Task<OrderResponseDto> GetOrderByIdAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        return _mapper.Map<OrderResponseDto>(order);
    }
    
    public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var exist = await _unitOfWork.OrderRepository.IsEntityExistsAsync(x => x.Code == model.Code);
        _validation.CheckConflict(exist, "Order with the same code already exists");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
        _validation.CheckNotFound(user, "User not found");
        var order = _mapper.Map<Order>(model);
        await _unitOfWork.OrderRepository.InsertAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OrderResponseDto>(order);
    }
    
    public async Task<OrderResponseDto> UpdateOrderAsync(string id, OrderRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        var exist = await _unitOfWork.OrderRepository.IsEntityExistsAsync(x => x.Code == model.Code && x.Id != id);
        _validation.CheckConflict(exist, "Order with the same code already exists");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
        _validation.CheckNotFound(user, "User not found");
        order = _mapper.Map(model, order);
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OrderResponseDto>(order);
    }
    
    public async Task<bool> DeleteOrderAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        await _unitOfWork.OrderRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}