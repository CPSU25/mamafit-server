using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class VoucherBatchRepository : GenericRepository<VoucherBatch>, IVoucherBatchRepository
{
    public VoucherBatchRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }

    public async Task<PaginatedList<VoucherBatch>> GetAllAsync(int index, int pageSize, string? search)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.BatchCode.Contains(search) || x.Description.Contains(search));
        }
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<List<VoucherBatch>> GetAllMyVoucherAsync(string userId)
    {
        var result = await _dbSet
            .Include(x => x.VoucherDiscounts)
            .AsNoTracking()
            .Where( x => x.VoucherDiscounts.Any(x => x.UserId == userId)).ToListAsync();

        return result;
    }

    public async Task<bool> IsBatchExistedAsync(string batchCode, string batchName)
    {
        return await _dbSet.AsNoTracking().AnyAsync(x => 
            x.BatchCode == batchCode && x.BatchName == batchName && !x.IsDeleted);
    }
}