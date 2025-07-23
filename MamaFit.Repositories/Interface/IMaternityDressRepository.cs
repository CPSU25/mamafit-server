using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IMaternityDressRepository : IGenericRepository<MaternityDress>
    {
        Task<PaginatedList<MaternityDress>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<MaternityDress?> GetById(string id);
    }
}
