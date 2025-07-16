using MamaFit.BusinessObjects.DTO.SizeDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface ISizeService
{
    Task<PaginatedList<SizeDto>> GetAllAsync(int index, int pageSize, string? search);
    Task<SizeDto> GetByIdAsync(string id);
    Task UpdateAsync(string id, SizeRequestDto requestDto);
    Task CreateAsync(SizeRequestDto requestDto);
    Task DeleteAsync(string id);
}