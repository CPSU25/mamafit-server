using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository;

public class VoucherDiscountRepository : GenericRepository<VoucherDiscount> , IVoucherDiscountRepository
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
}