using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IComponentService
    {
        Task<ComponentResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<ComponentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task CreateAsync(ComponentRequestDto requestDto);
        Task UpdateAsync(string id, ComponentRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
