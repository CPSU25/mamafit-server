using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository;

public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
{
    public CartItemRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
    
    public async Task<PaginatedList<CartItem>> GetAllAsync(int index, int pageSize)
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .GetPaginatedList(index, pageSize);
    }
}