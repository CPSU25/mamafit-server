using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IComponentService
    {
        Task<ComponentGetByIdResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<ComponentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task<List<ComponentGetByIdResponseDto>> GetComponentHavePresetByStyleId(string styleId);
        Task CreateAsync(ComponentRequestDto requestDto);
        Task UpdateAsync(string id, ComponentRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
