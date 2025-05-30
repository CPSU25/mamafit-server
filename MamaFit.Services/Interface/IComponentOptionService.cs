using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IComponentOptionService
    {
        Task<ComponentOptionResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<ComponentOptionResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task CreateAsync(ComponentOptionRequestDto requestDto);
        Task UpdateAsync(string id, ComponentOptionRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
