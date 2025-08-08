using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IWarrantyRequestService
    {
        Task<PaginatedList<WarrantyRequestGetAllDto>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<WarrantyRequestGetAllDto> GetWarrantyRequestByIdAsync(string id);
        Task<string> CreateAsync(WarrantyRequestCreateDto warrantyRequestCreateDto, string userId);
        // Task UpdateAsync(string id, WarrantyRequestUpdateDto warrantyRequestUpdateDto);
        Task DeleteAsync(string id);
    }
}
