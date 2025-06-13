using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IComponentRepository: IGenericRepository<Component>
    {
        Task<PaginatedList<Component>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task<Component> GetById(string id);
    }
}
