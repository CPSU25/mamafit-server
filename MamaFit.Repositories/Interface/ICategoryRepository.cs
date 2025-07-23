using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<PaginatedList<Category>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<Category> GetById(string id);
    }
}
