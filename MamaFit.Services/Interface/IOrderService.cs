using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using Newtonsoft.Json.Linq;

namespace MamaFit.Services.Interface;

public interface IOrderService
{
    Task<PaginatedList<OrderResponseDto>> GetByTokenAsync(string accessToken, int index = 1, int pageSize = 10,
        string? search = null, OrderStatus? status = null);
    Task<PaginatedList<OrderResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<OrderResponseDto> GetOrderByIdAsync(string id);
    Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto model);
    Task UpdateOrderStatusAsync(string id, OrderStatus orderStatus, PaymentStatus paymentStatus);
    Task<OrderResponseDto> UpdateOrderAsync(string id, OrderRequestDto model);
    Task<bool> DeleteOrderAsync(string id);
    Task<string> CreateReadyToBuyOrderAsync(OrderReadyToBuyRequestDto request);
    Task<string> CreateDesignRequestOrderAsync(OrderDesignRequestDto request);
    Task<string> CreatePresetOrderAsync(OrderPresetCreateRequestDto request);
    Task WebhookForContentfulWhenUpdateData(CmsServiceBaseDto request);

}