using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IMaternityDressService
    {
        Task<MaternityDressResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<MaternityDressGetAllResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? styleId, EntitySortBy? sortBy);
        Task CreateAsync(MaternityDressRequestDto requestDto);
        Task UpdateAsync(string id, MaternityDressRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
