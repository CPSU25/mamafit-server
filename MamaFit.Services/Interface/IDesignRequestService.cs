using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IDesignRequestService
    {
        Task CreateAsync(DesignRequestCreateDto requestDto);
        Task<PaginatedList<DesignResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
        Task<DesignResponseDto> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}
