using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IWarrantyRequestRepository : IGenericRepository<WarrantyRequest>
    {
        Task<List<WarrantyRequest>> GetAllWarrantyRequestByOrderId(string orderId);
        Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
    }
}
