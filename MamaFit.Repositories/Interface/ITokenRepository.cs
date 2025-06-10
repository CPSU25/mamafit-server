using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Repositories.Interface;

public interface ITokenRepository : IGenericRepository<ApplicationUserToken>
{
    Task<ApplicationUserToken?> GetTokenAsync(string token, TokenType tokenType);
    Task<ApplicationUserToken?> GetTokenByUserIdAsync(string userId, TokenType tokenType);
    Task CreateTokenAsync(ApplicationUserToken userToken);
    Task UpdateTokenAsync(ApplicationUserToken userToken);
    Task<bool> DeleteTokenAsync(string userId, string token, TokenType tokenType);
}