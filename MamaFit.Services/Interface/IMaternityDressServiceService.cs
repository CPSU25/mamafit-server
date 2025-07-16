using MamaFit.BusinessObjects.DTO.AddOnDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMaternityDressServiceService
{
    Task<PaginatedList<AddOnDto>> GetAllAsync(int index, int pageSize, string? search,
        EntitySortBy? sortBy);
    Task<AddOnDto> GetByIdAsync(string? id);
    Task UpdateAsync(string id, AddOnRequestDto requestDto);
    Task CreateAsync(AddOnRequestDto requestDto);
    Task DeleteAsync(string id);
}