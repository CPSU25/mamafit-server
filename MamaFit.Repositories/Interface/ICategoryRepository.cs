using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<PaginatedList<Category>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task<Category> GetById(string id);
    }
}
