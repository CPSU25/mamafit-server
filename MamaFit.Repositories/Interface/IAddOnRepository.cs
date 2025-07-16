using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IAddOnRepository : IGenericRepository<AddOn>
{
    Task<PaginatedList<AddOn>> GetAllAsync(int index, int pageSize, string? search,
        EntitySortBy? sortBy);
}