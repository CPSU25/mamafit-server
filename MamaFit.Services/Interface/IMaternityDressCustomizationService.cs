using MamaFit.BusinessObjects.DTO.MaternityDressCustomizationDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IMaternityDressCustomizationService
    {
        Task<PaginatedList<CustomResponseDto>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<CustomResponseDto> GetById(string id);
        Task CreateCustom(CustomCreateRequestDto request);
        Task UpdateCustom(string id, CustomUpdateRequestDto request);
        Task DeleteCustom(string id);
    }
}
