using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<PaginatedList<Branch>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    }
}
