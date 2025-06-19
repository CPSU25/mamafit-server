using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IVoucherDiscountRepository : IGenericRepository<VoucherDiscount>
{
    Task<PaginatedList<VoucherDiscount>> GetAllAsync(int index, int pageSize, string? CodeSearch);
}