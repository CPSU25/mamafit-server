using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IBranchService
    {
        Task<BranchResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<BranchResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task CreateAsync(BranchCreateDto requestDto);
        Task UpdateAsync(string id, BranchCreateDto requestDto);
        Task DeleteAsync(string id);
    }
}
