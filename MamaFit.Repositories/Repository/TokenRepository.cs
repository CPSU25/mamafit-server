using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class TokenRepository : GenericRepository<ApplicationUserToken>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor)
    {
    }

    public async Task<ApplicationUserToken?> GetTokenAsync(string token, TokenType tokenType)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Token == token && t.TokenType == tokenType);
    }
    
    public async Task<ApplicationUserToken?> GetTokenByUserIdAsync(string userId, TokenType tokenType)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.UserId == userId && t.TokenType == tokenType);
    }

    public async Task CreateTokenAsync(ApplicationUserToken userToken)
    {
        await InsertAsync(userToken);
    }

    public async Task UpdateTokenAsync(ApplicationUserToken userToken)
    {
        await UpdateAsync(userToken);
    }
    public async Task<bool> DeleteTokenAsync(string userId, string token, TokenType tokenType)
    {
        var userToken =
            await _dbSet.FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token && t.TokenType == tokenType);
        if (userToken != null)
        {
            await DeleteAsync(userToken);
            return true;
        }

        return false;
    }
}