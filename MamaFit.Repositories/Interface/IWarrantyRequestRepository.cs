using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IWarrantyRequestRepository : IGenericRepository<WarrantyRequest>
    {
        Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<WarrantyRequest> GetWarrantyRequestByIdAsync(string id);
        Task<int> CountWarrantyForOrderItemAsync(string orderItemId);
        Task<WarrantyRequest> GetWarrantyRequestByOrderItemIdAsync(string orderItemId);
    }
}
