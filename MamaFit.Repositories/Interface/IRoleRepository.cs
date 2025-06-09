using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IRoleRepository
{
    Task<PaginatedList<ApplicationUserRole>> GetRolesAsync(int index, int pageSize, string? nameSearch);
}