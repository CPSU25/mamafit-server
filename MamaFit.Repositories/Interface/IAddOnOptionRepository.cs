using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IAddOnOptionRepository : IGenericRepository<AddOnOption>
{
    Task<PaginatedList<AddOnOption>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
}