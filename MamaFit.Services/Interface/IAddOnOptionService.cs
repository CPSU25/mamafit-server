using MamaFit.BusinessObjects.DTO.AddOnOptionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IAddOnOptionService
{
    Task<PaginatedList<AddOnOptionDto>> GetAllAsync(int index, int pageSize, string? search,
        EntitySortBy? sortBy);
    Task<AddOnOptionDto> GetByIdAsync(string id);
    Task UpdateAsync(string id, AddOnOptionRequestDto requestDto);
    Task CreateAsync(AddOnOptionRequestDto requestDto);
    Task DeleteAsync(string id);
}