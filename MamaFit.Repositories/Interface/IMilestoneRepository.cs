using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IMilestoneRepository : IGenericRepository<Milestone>
    {
        Task<PaginatedList<Milestone>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<List<Milestone>> GetAllWithInclude();
        Task<Milestone> GetByIdDetailAsync(string id);
        Task<List<Milestone>> GetByOrderItemId(string orderItemId);
    }
}
