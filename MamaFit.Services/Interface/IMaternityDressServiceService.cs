using MamaFit.BusinessObjects.DTO.MaternityDressServiceDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMaternityDressServiceService
{
    Task<PaginatedList<MaternityDressServiceDto>> GetAllAsync(int index, int pageSize, string? search,
        EntitySortBy? sortBy);

    Task CreateAsync(MaternityDressServiceRequestDto requestDto);
}