using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Repositories.Interface;

public interface ITokenRepository
{
    Task<ApplicationUserToken> GetTokenAsync(string token, TokenType tokenType);
    Task CreateTokenAsync(ApplicationUserToken userToken);
    Task UpdateTokenAsync(ApplicationUserToken userToken);
    Task<bool> DeleteTokenAsync(string userId, string token, TokenType tokenType);
}