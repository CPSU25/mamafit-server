using MamaFit.BusinessObjects.DTO.MaternityDressTask;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IMaternityDressTaskService
    {
        Task<PaginatedList<MaternityDressTaskResponseDto>> GetAllAsync(int index, int pageSize, string search, EntitySortBy? sortBy);
        Task<MaternityDressTaskResponseDto> GetById(string? id);
        Task CreateAsync(MaternityDressTaskRequestDto request);
        Task UpdateAsync(string id, MaternityDressTaskRequestDto request);
        Task DeleteAsync(string id);
    }
}
