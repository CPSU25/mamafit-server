using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IPresetService
    {
        Task<PaginatedList<PresetGetAllResponseDto>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<PresetGetByIdResponseDto> GetById(string id);
        public Task<PresetGetByIdResponseDto> GetDefaultPresetByStyleId(string styleId);
        public Task<List<PresetGetByIdResponseDto>> GetPresetByDesignRequestId(string designRequestId);
        public Task<List<PresetGetByIdResponseDto>> GetAllPresetByComponentOptionId(List<string> componentOptionIds);
        public Task<PaginatedList<PresetRatedResponseDto>> GetMostSelledPreset(int index, int pageSize, DateTime? startDate, DateTime? endDate, OrderStatus? filterBy);
        Task CreatePresetAsync(PresetCreateRequestDto request);
        Task<string> CreatePresetForDesignRequestAsync(PresetCreateForDesignRequestDto request);
        Task UpdatePresetAsync(string id, PresetUpdateRequestDto request);
        Task DeletePresetAsync(string id);
    }
}
