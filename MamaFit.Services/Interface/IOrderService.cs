using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using Newtonsoft.Json.Linq;

namespace MamaFit.Services.Interface;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetOrdersForDesignerAsync();
    Task<List<OrderResponseDto>> GetOrdersForBranchManagerAsync();
    Task<List<OrderResponseDto>> GetOrdersForAssignedStaffAsync();
    Task<OrderResponseDto> GetBySkuAndCodeAsync(string code, string? sku = null);
    Task<PaginatedList<OrderResponseDto>> GetByTokenAsync(string accessToken, int index = 1, int pageSize = 10,
        string? search = null, OrderStatus? status = null);
    Task<PaginatedList<OrderResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<List<OrderGetByIdResponseDto>> GetAllByDesignRequestId(string designRequestId);
    Task<OrderGetByIdResponseDto> GetOrderByIdAsync(string id);
    Task<OrderGetByIdResponseDto> GetOrderByIdForFeedbackAsync(string id);
    Task<List<OrderGetByIdResponseDto>> GetForWarranty();
    Task<List<MyOrderStatusCount>> GetMyOrderStatusCounts();
    Task<List<OrderResponseDto>> GetOrderToFeedback();
    Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto model);
    Task OrderReceivedAtUpdateAsync(string orderId);
    Task UpdateOrderStatusAsync(string id, OrderStatus orderStatus, PaymentStatus paymentStatus);
    Task<OrderResponseDto> UpdateOrderAsync(string id, OrderRequestDto model);
    Task UpdateReceivedOrderAsync(string id);
    Task UpdateCancelledOrderAsync(string id, string? cancelReason = null);
    Task<bool> DeleteOrderAsync(string id);
    Task<string> CreateReadyToBuyOrderAsync(OrderReadyToBuyRequestDto request);
    Task<string> CreateDesignRequestOrderAsync(OrderDesignRequestDto request);
    Task<string> CreatePresetOrderAsync(OrderPresetCreateRequestDto request);
    Task WebhookForContentfulWhenUpdateData(CmsServiceBaseDto request);
    Task ReceivedAtBranchAsync(string orderId);

}