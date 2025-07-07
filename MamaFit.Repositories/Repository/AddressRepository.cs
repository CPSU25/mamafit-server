using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context,
            httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Address>> GetAllAsync(int index, int pageSize)
        {
            var query = _dbSet.Where(x => !x.IsDeleted);
            return await query.GetPaginatedList(index, pageSize);
        }

        public async Task<List<Address>> GetByUserId(string userId)
        {
            return await _dbSet.Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task SetDefaultFalseForAllAsync(string userId)
        {
            var addresses = await _dbSet.Where(a => a.UserId == userId && a.IsDefault)
                .ToListAsync();
            foreach (var address in addresses)
            {
                address.IsDefault = false;
            }

            _dbSet.UpdateRange(addresses);
            await _context.SaveChangesAsync();
        }
    }
}