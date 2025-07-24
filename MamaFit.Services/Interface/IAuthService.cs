using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.TokenDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.Services.Interface;

public interface IAuthService
{
    Task<PermissionResponseDto> GetCurrentUserAsync();
    Task CreateSystemAccountAsync(SystemAccountRequestDto model);
    Task VerifyPhoneOtpAsync(VerifyPhoneOtpDto model);
    Task UpdatePhoneNumberAsync(PhoneNumberRequestDto model);
    Task<TokenResponseDto> SignInAsync(LoginRequestDto model);
    Task LogoutAsync(LogoutRequestDto model);
    Task ResendOtpAsync(SendOTPRequestDto model);
    Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model);
    Task CompleteRegisterAsync(RegisterUserRequestDto model);
    Task SendRegisterOtpAsync(SendOTPRequestDto model);
    Task VerifyOtpAsync(VerifyOtpRequestDto model);
    GoogleJwtPayload DecodePayload(string jwtToken);
    Task<TokenResponseDto> SignInWithGoogleJwtAsync(string jwtToken, string? notificationToken = null);
}