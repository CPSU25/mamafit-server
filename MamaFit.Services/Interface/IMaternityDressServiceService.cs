using MamaFit.BusinessObjects.DTO.MaternityDressServiceDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMaternityDressServiceService
{
    Task<PaginatedList<MaternityDressServiceDto>> GetAllAsync(int index, int pageSize, string? search,
        EntitySortBy? sortBy);
    Task<MaternityDressServiceDto> GetByIdAsync(string? id);
    Task UpdateAsync(string id, MaternityDressServiceRequestDto requestDto);
    Task CreateAsync(MaternityDressServiceRequestDto requestDto);
    Task DeleteAsync(string id);
}