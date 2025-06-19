using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IMaternityDressTaskRepository : IGenericRepository<MaternityDressTask>
    {
        Task<PaginatedList<MaternityDressTask>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<MaternityDressTask> GetByIdAsync(string id);
    }
}
