using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IRoleRepository
{
    Task<PaginatedList<ApplicationUserRole>> GetRolesAsync(int index, int pageSize, string? nameSearch);
    Task<ApplicationUserRole?> GetByIdAsync(string id);
    Task<ApplicationUserRole?> GetByNameAsync(string name);
    Task<bool> IsRoleNameExistedAsync(string name);
    Task CreateAsync(ApplicationUserRole role);
    Task UpdateRoleAsync(ApplicationUserRole role);
    Task DeleteAsync(ApplicationUserRole role);
}