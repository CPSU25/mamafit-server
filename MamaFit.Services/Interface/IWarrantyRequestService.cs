﻿using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface
{
    public interface IWarrantyRequestService
    {
        Task<PaginatedList<WarrantyRequestGetAllDto>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<WarrantyGetByIdResponseDto> GetWarrantyRequestByIdAsync(string id);
        Task<string> CreateAsync(WarrantyRequestCreateDto warrantyRequestCreateDto, string userId);
        Task<string> CreateRequestInBranch(WarrantyBranchRequestDto dto, string accessToken);
        Task CompleteWarrantyOrderAsync(string orderId);
        Task CompleteWarrantyRequestAsync(string warrantyRequestId);
        // Task UpdateAsync(string id, WarrantyRequestUpdateDto warrantyRequestUpdateDto);
        Task DeleteAsync(string id);
        Task<WarrantyDetailResponseDto> DetailsByIdAsync(string orderId);
        Task<WarrantyDecisionResponseDto> DecideAsync(string warrantyRequestId,
            WarrantyDecisionRequestDto dto);
        Task<WarrantyDecisionResponseDto> ShipPaidWarrantyAsync(string warrantyRequestId);
        Task<PaginatedList<WarrantyRequestGetAllDto>> GetAllWarrantyRequestOfBranchAsync(int index, int pageSize, string? search, EntitySortBy? sortBy, string accessToken);
        Task AssignWarrantyTasksAfterPaidAsync(Order order);
    }
}
