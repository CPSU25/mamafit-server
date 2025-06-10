using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class RoleRepository : GenericRepository<ApplicationUserRole>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context, IHttpContextAccessor accessor) 
        : base(context, accessor)
    {
    }
    
    public async Task<PaginatedList<ApplicationUserRole>> GetRolesAsync(int index, int pageSize, string? nameSearch)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);
        
        if (!string.IsNullOrWhiteSpace(nameSearch))
            query = query.Where(x => x.RoleName.Contains(nameSearch));
        
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<ApplicationUserRole?> GetByNameAsync(string name)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoleName == name && !r.IsDeleted);
    }
    public async Task<ApplicationUserRole?> GetByIdAsync(string id)
    {
        return await GetByIdNotDeletedAsync(id);
    }

    public async Task<bool> IsRoleNameExistedAsync(string name)
    {
        return await _dbSet.AsNoTracking().AnyAsync(r =>
            r.RoleName == name && !r.IsDeleted);
    }
    
    public async Task CreateAsync(ApplicationUserRole role)
    {
        await InsertAsync(role);
    }

    public async Task UpdateRoleAsync(ApplicationUserRole role)
    {
        await UpdateAsync(role);
    }
    
    public async Task DeleteAsync(ApplicationUserRole role)
    {
        await SoftDeleteAsync(role.Id);
    }
}