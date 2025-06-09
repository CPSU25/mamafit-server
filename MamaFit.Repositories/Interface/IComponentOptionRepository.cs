using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IComponentOptionRepository: IGenericRepository<ComponentOption>
    {
        Task<PaginatedList<ComponentOption>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    }
}
