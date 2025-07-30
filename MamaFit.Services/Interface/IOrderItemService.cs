using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IOrderItemService
{
    Task<PaginatedList<OrderItemResponseDto>> GetAllOrderItemsAsync(int index, int pageSize, DateTime? startDate,
        DateTime? endDate);
    Task<OrderItemGetByIdResponseDto> GetOrderItemByIdAsync(string id);
    Task<OrderItemResponseDto> CreateOrderItemAsync(OrderItemRequestDto model);
    Task<OrderItemResponseDto> UpdateOrderItemAsync(string id, OrderItemRequestDto model);
    Task<DesignerInfoOrderItemResponseDto> GetDesignerInfoByOrderItemIdAsync(string orderItemId);
    Task DeleteOrderItemAsync(string id);
    Task AssignTaskToOrderItemAsync(AssignTaskToOrderItemRequestDto request);
    Task AssignChargeToOrderItemAsync(AssignChargeToOrderItemRequestDto request, ApplicationUser user);
    Task AssignChargeToOrderItemListAsync(List<AssignChargeToOrderItemRequestDto> requests);
    Task CheckListStatusForOrderItemTaskAsync(OrderItemCheckTaskRequestDto request);
}