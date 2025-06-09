using System.Linq.Expressions;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{

    public UserRepository(ApplicationDbContext context, IHttpContextAccessor accessor) 
        : base(context, accessor)
    {
    }

    
}