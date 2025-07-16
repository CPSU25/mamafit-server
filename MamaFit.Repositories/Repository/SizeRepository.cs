using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class SizeRepository : GenericRepository<Size>, ISizeRepository
{
    public SizeRepository(ApplicationDbContext context, IHttpContextAccessor accessor) 
        : base(context, accessor)
    {
    }
    
    public async Task<PaginatedList<Size>> GetAllAsync(int index, int pageSize, string? search)
    {
        var query = _dbSet.AsNoTracking()
            .Where(s => !s.IsDeleted);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(s => s.Name.Contains(search));
        }

        return await GetPaging(query, index, pageSize);
    }
}