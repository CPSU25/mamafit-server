using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryGetByIdResponse> GetByIdAsync(string id);
        Task<PaginatedList<CategoryResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task CreateAsync(CategoryRequestDto requestDto);
        Task UpdateAsync(string id, CategoryRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
