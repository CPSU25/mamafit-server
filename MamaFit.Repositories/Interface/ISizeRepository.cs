using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface ISizeRepository : IGenericRepository<Size>
{
    Task<PaginatedList<Size>> GetAllAsync(int index, int pageSize, string? search);
}