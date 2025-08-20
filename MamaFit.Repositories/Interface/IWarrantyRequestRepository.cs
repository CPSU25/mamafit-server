using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IWarrantyRequestRepository : IGenericRepository<WarrantyRequest>
    {
        Task<List<WarrantyRequest>> GetAllWarrantyRequest();
        Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<WarrantyRequest?> GetDetailById(string warrantyId);
        Task<List<WarrantyRequest>> GetFeeWarrantyRequestsByOrderIdAsync(string orderId);
        Task<PaginatedList<WarrantyRequest>> GetAllWarrantyRequestOfBranchAsync(int index, int pageSize, string? search, EntitySortBy? sortBy, string branchId);
    }
}
