using System.Linq.Expressions;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor)
    {
    }

    public async Task<PaginatedList<ApplicationUser>> GetUsersAsync(int index, int pageSize, string? nameSearch,
        string? roleName)
    {
        var roles = _dbSet
            .Include(x => x.Role)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nameSearch))
            roles = roles.Where(x => x.UserName.Contains(nameSearch));

        if (!string.IsNullOrWhiteSpace(roleName))
        {
            roles = roles.Where(x => x.Role.RoleName.Contains(roleName));
        }

        return await roles.GetPaginatedList(index, pageSize);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        var user = await _dbSet
            .Include(x => x.Role)
            .Include(x => x.Appointments)
            .Include(x => x.Orders)
            .Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<ApplicationUser?> GetByEmailPhoneAsync(string email, string phone)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.UserEmail.ToLower() == email && x.PhoneNumber == phone && !x.IsDeleted);
    }
    public async Task<ApplicationUser?> GetByPhoneNumberAsync(string phoneNumber) 
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && !x.IsDeleted);
    }
    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.UserEmail.ToLower() == email && !x.IsDeleted);
    }

    public async Task<ApplicationUser?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.UserName == username && !x.IsDeleted);
    }
    
    public async Task CreateUserAsync(ApplicationUser user)
    {
        await InsertAsync(user);
    }
    public async Task UpdateUserAsync(ApplicationUser user)
    {
        await UpdateAsync(user);
    }
    
    public async Task DeleteUserAsync(ApplicationUser user)
    {
        await SoftDeleteAsync(user.Id);
    }
}