using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IBranchMaternityDressDetailRepository
{
    Task<PaginatedList<BranchMaternityDressDetail>> GetAllAsync(int index, int pageSize, string? search);
    Task<BranchMaternityDressDetail?> GetByIdAsync(string branchId, string dressDetailId);
    Task InsertAsync(BranchMaternityDressDetail entity);
    Task UpdateAsync(BranchMaternityDressDetail entity);
    Task DeleteAsync(string branchId, string dressDetailId);
}