using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;


namespace MamaFit.Repositories.Interface
{
    public interface IStyleRepository : IGenericRepository<Style>
    {
        Task<PaginatedList<Style>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    }
}
