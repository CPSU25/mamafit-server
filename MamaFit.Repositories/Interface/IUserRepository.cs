using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IUserRepository
{
    Task<PaginatedList<ApplicationUser>> GetUsersAsync(int index, int pageSize, string? nameSearch, string? roleName);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<ApplicationUser?> GetByUsernameAsync(string username);
    Task<ApplicationUser?> GetByEmailPhoneAsync(string email, string phoneNumber);
    Task CreateUserAsync(ApplicationUser user);
    Task UpdateUserAsync(ApplicationUser user);
    Task DeleteUserAsync(ApplicationUser user);
}