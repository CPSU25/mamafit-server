using MamaFit.BusinessObjects.DTO.PositionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IPositionService
{
    Task<PaginatedList<PositionDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
    Task CreateAsync(PositionRequestDto requestDto);
    Task<PositionDto> GetByIdAsync(string id);
    Task UpdateAsync(string id, PositionRequestDto requestDto);
    Task DeleteAsync(string id);
}