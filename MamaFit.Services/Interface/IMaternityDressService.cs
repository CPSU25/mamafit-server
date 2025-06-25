using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IMaternityDressService
    {
        Task<MaternityDressResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<MaternityDressGetAllResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task CreateAsync(MaternityDressRequestDto requestDto);
        Task UpdateAsync(string id,MaternityDressRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
