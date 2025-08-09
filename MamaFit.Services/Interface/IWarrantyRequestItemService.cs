using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IWarrantyRequestItemService
{
    Task<PaginatedList<WarrantyRequestItemGetAllDto>> GetAllAsync(int index, int pageSize, string? search);
    Task<WarrantyRequestItemDetailDto> GetDetailsByOrderItemIdAsync(string orderItemId);
}