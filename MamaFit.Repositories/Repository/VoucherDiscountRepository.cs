using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
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
        var query = _dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(codeSearch))
            query = query.Where(x => x.Code!.Contains(codeSearch));
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<List<VoucherDiscount>> GetAllByCurrentUserAsync(string userId)
    {
        var query = await _dbSet
            .Include(x => x.VoucherBatch)
            .Where(x => x.UserId == userId && x.Status == VoucherStatus.ACTIVE)
            .ToListAsync();

        return query;
    }

    public async Task<VoucherDiscount?> GetVoucherDiscountWithBatch(string id)
    {
         return await _dbSet.Include(x => x.VoucherBatch)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}