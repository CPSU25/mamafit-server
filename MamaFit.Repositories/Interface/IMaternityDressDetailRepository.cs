using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IMaternityDressDetailRepository : IGenericRepository<MaternityDressDetail>
    {
        Task<PaginatedList<MaternityDressDetail>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<PaginatedList<MaternityDressDetail>> GetAllByMaternityDressId(string maternityDressId, int index, int pageSize, string? search, EntitySortBy? sortBy);
    }
}
