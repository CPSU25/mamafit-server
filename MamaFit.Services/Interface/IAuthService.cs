using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.Token;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.Services.Interface;

public interface IAuthService
{
    Task<UserReponseDto> GetCurrentUserAsync();
    Task<TokenResponseDto> SignInAsync(LoginRequestDto model);
    Task LogoutAsync(LogoutRequestDto model);
    Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model);
    TokenResponseDto GenerateTokens(ApplicationUser user, string role);
    Task VerifyOtpAsync(VerifyOtpRequestDto model);
    GoogleJwtPayload DecodePayload(string jwtToken);
    Task<TokenResponseDto> SignInWithGoogleJwtAsync(string jwtToken, string? notificationToken = null);
}