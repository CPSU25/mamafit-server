using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IWarrantyRequestItemRepository
{
    Task InsertAsync(WarrantyRequestItem entity);
    Task<WarrantyRequestItem?> GetByIdAsync(string itemId, string requestId);
    Task<PaginatedList<WarrantyRequestItem>> GetAllAsync(int index, int pageSize, string? search);
    Task<int> CountWarrantyRequestItemsAsync(string requestId);
    Task<WarrantyRequestItem> GetByOrderItemIdAsync(string orderItemId);
    Task<List<WarrantyRequestItem>> GetAllRelatedByOrderItemAsync(string orderItemId);
    Task UpdateAsync(WarrantyRequestItem entity);
    Task<Dictionary<string, int>> GetWarrantyRoundsByOriginalOrderItemIdsAsync(List<string> orderItemIds);
}