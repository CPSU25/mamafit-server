using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IVoucherBatchRepository : IGenericRepository<VoucherBatch>
{
    Task<PaginatedList<VoucherBatch>> GetAllAsync(int index, int pageSize, string? search);
    Task<bool> IsBatchExistedAsync(string batchCode, string batchName);
}