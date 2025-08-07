using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IWarrantyRequestService
    {
        Task<PaginatedList<WarrantyRequestGetAllDto>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        // Task<WarrantyRequestGetByIdDto> GetWarrantyRequestByIdAsync(string id);
        // Task<GetDetailDto> GetWarrantyRequestByOrderItemIdAsync(string orderItemId);
        Task<string> CreateAsync(WarrantyRequestCreateDto warrantyRequestCreateDto);
        // Task UpdateAsync(string id, WarrantyRequestUpdateDto warrantyRequestUpdateDto);
        Task DeleteAsync(string id);
    }
}
