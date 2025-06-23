using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IOrderService
{
    Task<PaginatedList<OrderResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<OrderResponseDto> GetOrderByIdAsync(string id);
    Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto model);
    Task<OrderResponseDto> UpdateOrderAsync(string id, OrderRequestDto model);
    Task<bool> DeleteOrderAsync(string id);
    Task CreateReadyToBuyOrderAsync(OrderReadyToBuyRequestDto request);
    Task CreateDesignRequestAsync(OrderDesignRequestDto request);
}