using AutoMapper;
using MamaFit.BusinessObjects.DTO.FeedbackDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class FeedbackService : IFeedbackService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _contextAccessor;

    public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
    }

    public async Task<PaginatedList<FeedbackResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate,
        DateTime? endDate)
    {
        var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync(index, pageSize, startDate, endDate);

        var responseItems = feedbacks.Items
            .Select(feedback => _mapper.Map<FeedbackResponseDto>(feedbacks))
            .ToList();

        return new PaginatedList<FeedbackResponseDto>(
            responseItems,
            feedbacks.TotalCount,
            feedbacks.PageNumber,
            pageSize
        );
    }

    public async Task<FeedbackResponseDto> GetByIdAsync(string id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(feedback, "Feedback not found");
        return _mapper.Map<FeedbackResponseDto>(feedback);
    }

    public async Task CreateAsync(FeedbackRequestDto requestDto)
    {

        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, "Please sign in");

        var orderItem = await _unitOfWork.OrderItemRepository.GetByIdAsync(requestDto.OrderItemId);
        _validation.CheckNotFound(orderItem, $"Order item with id: {requestDto.OrderItemId} not found");

        var oldFeedback = await _unitOfWork.FeedbackRepository.GetFeedbackByUserIdAndOrderItemId(userId, orderItem.Id);
        if (oldFeedback != null)
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, $"You left a feedback for order item with id: {orderItem.Id} already");

        var entity = _mapper.Map<Feedback>(requestDto);
        entity.User = user;
        entity.UserId = userId;
        await _unitOfWork.FeedbackRepository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(string id, FeedbackRequestDto requestDto)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(feedback, "Feedback not found");

        var orderItem = await _unitOfWork.OrderItemRepository.GetByIdAsync(requestDto.OrderItemId);
        _validation.CheckNotFound(orderItem, $"Order item with id: {requestDto.OrderItemId} not found");

        _mapper.Map(requestDto, feedback);

        await _unitOfWork.FeedbackRepository.UpdateAsync(feedback);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(feedback, "Feedback not found");

        await _unitOfWork.FeedbackRepository.SoftDeleteAsync(feedback);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<OrderItemResponseDto>> GetAllByUserId()
    {
        var currentUserId = GetCurrentUserId();

        var feedbacks = await _unitOfWork.FeedbackRepository.GetAllByUserId(currentUserId);
        var orderItems = feedbacks
            .Where(f => f.OrderItem != null)
            .Select(f => f.OrderItem)
            .Distinct()
            .ToList();

        return _mapper.Map<List<OrderItemResponseDto>>(orderItems);
    }

    public async Task<List<OrderItemResponseDto>> GetAllByDressId(string dressId)
    {
        var feedbacks = await _unitOfWork.FeedbackRepository.GetAllByDressId(dressId);
        var orderItems = feedbacks
            .Where(f => f.OrderItem != null)
            .Select(f => f.OrderItem)
            .Distinct()
            .ToList();

        return _mapper.Map<List<OrderItemResponseDto>>(orderItems);
    }

    public async Task<bool> CheckFeedbackByOrderId(string orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithItems(orderId);
        _validation.CheckNotFound(order, $"Order with id: {orderId} not found");
        if (order.OrderItems.All(x => x.Feedbacks.Count() <= 0))
        {
            return false;
        }
        return true;
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirst("userId").Value;
    }
}