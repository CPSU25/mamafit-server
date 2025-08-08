using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IWarrantyRequestItemRepository
{
    Task InsertAsync(WarrantyRequestItem entity);
    Task<WarrantyRequestItem?> GetByIdAsync(string itemId, string requestId);
    Task<PaginatedList<WarrantyRequestItem>> GetAllAsync(int index, int pageSize, string? search);
    Task<int> CountWarrantyRequestItemsAsync(string requestId);
}