using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IComponentRepository: IGenericRepository<Component>
    {
        Task<PaginatedList<Component>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    }
}
