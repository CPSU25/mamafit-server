using MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IWarrantyHistoryService
{
    Task<PaginatedList<WarrantyHistoryResponseDto>> GetAllAsync(int index = 1, int pageSize = 10,
        DateTime? startDate = null, DateTime? endDate = null);
    Task<WarrantyHistoryResponseDto> GetByIdAsync(string id);
    Task<WarrantyHistoryResponseDto> CreateAsync(WarrantyHistoryRequestDto requestDto);
    Task<WarrantyHistoryResponseDto> UpdateAsync(string id, WarrantyHistoryRequestDto requestDto);
    Task DeleteAsync(string id);
}