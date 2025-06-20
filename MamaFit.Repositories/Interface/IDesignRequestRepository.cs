using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IDesignRequestRepository : IGenericRepository<DesignRequest>
    {
        Task<PaginatedList<DesignRequest>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    }
}
