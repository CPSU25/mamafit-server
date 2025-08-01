﻿using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IStyleService
    {
        Task<StyleGetByIdResponseDto> GetByIdAsync(string id);
        Task<PaginatedList<StyleResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<PaginatedList<StyleResponseDto>> GetAllByCategoryAsync(string categoryId, int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task CreateAsync(StyleRequestDto requestDto);
        Task UpdateAsync(string id, StyleRequestDto requestDto);
        Task DeleteAsync(string id);
    }
}
