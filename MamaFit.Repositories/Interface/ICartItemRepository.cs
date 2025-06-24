using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface ICartItemRepository : IGenericRepository<CartItem>
{
    Task<PaginatedList<CartItem>> GetAllAsync(int index, int pageSize);
}