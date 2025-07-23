using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IComponentOptionService
    {
        Task<ComponentOptionResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<ComponentOptionResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task CreateAsync(ComponentOptionRequestDto requestDto);
        Task UpdateAsync(string id, ComponentOptionRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
