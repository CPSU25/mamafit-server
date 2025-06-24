using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IMaternityDressSelectionRepository : IGenericRepository<MaternityDressSelection>
    {
        Task<PaginatedList<MaternityDressSelection>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<MaternityDressSelection?> GetDetailById(string id);
    }
}
