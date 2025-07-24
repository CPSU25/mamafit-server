using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class VoucherDiscountRepository : GenericRepository<VoucherDiscount>, IVoucherDiscountRepository
{
    public VoucherDiscountRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }

    public async Task<PaginatedList<VoucherDiscount>> GetAllAsync(int index, int pageSize, string? codeSearch)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(codeSearch))
            query = query.Where(x => x.Code.Contains(codeSearch));
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<List<VoucherDiscount>> GetAllByCurrentUserAsync(string userId)
    {
        var query = await _dbSet
            .Include(x => x.VoucherBatch)
            .Where(x => x.UserId == userId && !x.IsDeleted && x.Status == BusinessObjects.Enum.VoucherStatus.ACTIVE)
            .ToListAsync();

        return query;
    }

    public async Task<VoucherDiscount> GetVoucherDiscountWithBatch(string id)
    {
        var voucherDiscount = await _dbSet.Include(x => x.VoucherBatch)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (voucherDiscount == null)
        {
            throw new KeyNotFoundException("Voucher discount not found");
        }
        return voucherDiscount;
    }
}