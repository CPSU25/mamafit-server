using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IMaternityDressDetailRepository : IGenericRepository<MaternityDressDetail>
    {
        Task<PaginatedList<MaternityDressDetail>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    }
}
