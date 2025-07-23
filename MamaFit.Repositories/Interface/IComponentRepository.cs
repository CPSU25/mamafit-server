using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IComponentRepository: IGenericRepository<Component>
    {
        Task<PaginatedList<Component>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<Component> GetById(string id);
        Task<List<Component>> GetComponentHavePresetByStyleId(string styleId);
    }
}
