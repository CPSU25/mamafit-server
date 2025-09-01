using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
namespace MamaFit.Repositories.Interface;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<PaginatedList<ApplicationUser>> GetUsersAsync(int index, int pageSize, string? nameSearch, string? roleName);
    Task<List<ApplicationUser>> GetAllUserAsync();
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<ApplicationUser?> GetByUsernameAsync(string username);
    Task<ApplicationUser?> GetByEmailPhoneAsync(string email, string phoneNumber);
    Task<ApplicationUser?> GetByPhoneNumberAsync(string phoneNumber);
    Task<List<ApplicationUser>> GetUsersByRoleIdAsync(string roleId, bool onlyActiveUsers = true);
    Task<List<string>> GetAllUserIdsByCustomerRoleAsync();
    Task CreateUserAsync(ApplicationUser user);
    Task UpdateUserAsync(ApplicationUser user);
    Task DeleteUserAsync(ApplicationUser user);
}