using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IMaternityDressDetailService
    {
        Task<MaternityDressDetailResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<MaternityDressDetailResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task CreateAsync(MaternityDressDetailRequestDto requestDto);
        Task UpdateAsync(string id, MaternityDressDetailRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
