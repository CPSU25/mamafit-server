using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
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
         .Where(x => x.VoucherDiscounts.Any(v => v.UserId == userId && v.Status == VoucherStatus.ACTIVE))
         .Select(x => new VoucherBatch
         {
             BatchName = x.BatchName,
             BatchCode = x.BatchCode,
             Description = x.Description,
             StartDate = x.StartDate,
             EndDate = x.EndDate,
             TotalQuantity = x.TotalQuantity,
             RemainingQuantity = x.RemainingQuantity,
             DiscountType = x.DiscountType,
             DiscountValue = x.DiscountValue,
             MinimumOrderValue = x.MinimumOrderValue,
             MaximumDiscountValue = x.MaximumDiscountValue,
             IsAutoGenerate = x.IsAutoGenerate ?? false,

             VoucherDiscounts = x.VoucherDiscounts
                 .Where(v => v.UserId == userId && v.Status == VoucherStatus.ACTIVE)
                 .ToList()
         })
         .AsNoTracking()
         .ToListAsync();

        return result;
    }

    public Task<VoucherBatch> GetDetailVoucherBatchAsync(string id)
    {
        var voucherBatch = _dbSet
            .Include(x => x.VoucherDiscounts)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        return voucherBatch;
    }

    public async Task<bool> IsBatchExistedAsync(string batchCode, string batchName)
    {
        return await _dbSet.AsNoTracking().AnyAsync(x =>
            x.BatchCode == batchCode && x.BatchName == batchName && !x.IsDeleted);
    }
}