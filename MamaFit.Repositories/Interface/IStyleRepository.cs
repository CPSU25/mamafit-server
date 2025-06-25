using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;


namespace MamaFit.Repositories.Interface
{
    public interface IStyleRepository : IGenericRepository<Style>
    {
        Task<PaginatedList<Style>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task<PaginatedList<Style>> GetAllByCategoryAsync(string categoryId,int index, int pageSize, string? search, string? sortBy);
        Task<Style> GetDetailById(string id);
    }
}
