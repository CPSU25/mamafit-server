﻿using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IPresetService
    {
        Task<PaginatedList<PresetGetAllResponseDto>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<PresetGetByIdResponseDto> GetById(string id);
        Task CreatePresetAsync(PresetCreateRequestDto request);
        Task UpdatePresetAsync(string id, PresetUpdateRequestDto request);
        Task DeletePresetAsync(string id);
    }
}
