using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IMilestoneService 
    {
        Task<PaginatedList<MilestoneResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<MilestoneGetByIdResponseDto> GetByIdAsync(string? id);
        Task CreateAsync(MilestoneRequestDto request);
        Task UpdateAsync(string id, MilestoneRequestDto request);
        Task DeleteAsync(string id);
    }
}
