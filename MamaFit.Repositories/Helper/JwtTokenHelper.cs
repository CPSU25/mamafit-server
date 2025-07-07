using System.IdentityModel.Tokens.Jwt;

namespace MamaFit.Repositories.Helper;

public static class JwtTokenHelper
{
    public static string ExtractUserId(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new UnauthorizedAccessException("Access token is missing");
        if (accessToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            accessToken = accessToken.Substring("Bearer ".Length).Trim();
        }
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(accessToken))
            throw new UnauthorizedAccessException("Invalid access token format");
        var jwtToken = handler.ReadJwtToken(accessToken);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Invalid access token: userId not found");
        return userIdClaim.Value;
    }
}