using MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IBranchMaternityDressDetailService
{
    Task<PaginatedList<BranchMaternityDressDetailDto>> GetAllAsync(int index, int pageSize, string accessToken, string? search);
    Task<GetDetailById> GetByIdAsync(string branchId, string dressId);
    Task<BranchMaternityDressDetailDto> CreateAsync(BranchMaternityDressDetailDto request);
    Task<BranchMaternityDressDetailDto> UpdateAsync(BranchMaternityDressDetailDto request);
    Task DeleteAsync(string branchId, string dressId);
}