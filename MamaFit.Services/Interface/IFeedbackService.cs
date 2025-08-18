using MamaFit.BusinessObjects.DTO.FeedbackDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IFeedbackService
{
    Task<PaginatedList<FeedbackResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate,
        DateTime? endDate);

    Task<FeedbackResponseDto> GetByIdAsync(string id);
    Task<List<OrderItemResponseDto>> GetAllByUserId();
    Task CreateAsync(FeedbackRequestDto requestDto);
    Task UpdateAsync(string id, FeedbackRequestDto requestDto);
    Task DeleteAsync(string id);
}