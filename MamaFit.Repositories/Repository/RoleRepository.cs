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
        var query = _dbSet.AsQueryable().Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nameSearch))
            query = query.Where(x => x.RoleName.Contains(nameSearch));

        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<ApplicationUserRole?> GetByIdIfNotDeletedAsync(string id)
    {
        var role = await _dbSet.FindAsync(id);
        return (role != null && !role.IsDeleted) ? role : null;
    }

    public async Task<bool> IsRoleNameExistedAsync(string name, string? excludeId = null)
    {
        return await _dbSet.AnyAsync(r =>
            r.RoleName == name && !r.IsDeleted &&
            (excludeId == null || r.Id != excludeId));
    }
}