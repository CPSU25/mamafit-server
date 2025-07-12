using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.Services.Interface;
using MamaFit.Repositories.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.TokenDto;
using Microsoft.AspNetCore.Authorization;
using MamaFit.BusinessObjects.DTO.UserDto;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("permission")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync();
            return Ok(new ResponseModel<PermissionResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                user,
                "Get current user successfully!"
            ));
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOTPRequestDto model)
        {
            await _authService.SendRegisterOtpAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Send OTP successfully. Please check your email!"
            ));
        }


        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtpAsync([FromBody] SendOTPRequestDto model)
        {
            await _authService.ResendOtpAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Resend OTP successfully!"
            ));
        }


        [HttpPost("complete-register")]
        public async Task<IActionResult> CompleteRegister([FromBody] RegisterUserRequestDto model)
        {
            await _authService.CompleteRegisterAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                null,
                "Register user successfully!"));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDto model)
        {
            var token = await _authService.SignInAsync(model);
            return Ok(new ResponseModel<TokenResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                token,
                "Login successfully!"
            ));
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginRequestDto request)
        {
            var tokenResponse =
                await _authService.SignInWithGoogleJwtAsync(request.JwtToken, request.NotificationToken);

            var response = new ResponseModel<TokenResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                tokenResponse,
                "Login successfully!"
            );

            return Ok(response);
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto model)
        {
            await _authService.LogoutAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Logout successfully!"
            ));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto model)
        {
            var token = await _authService.RefreshTokenAsync(model);
            return Ok(new ResponseModel<TokenResponseDto>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                token,
                "Refresh token successfully!"
            ));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto model)
        {
            await _authService.VerifyOtpAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "OTP verification successful!"
            ));
        }

        [HttpPost("decode")]
        public IActionResult DecodeJwt([FromBody] string jwtToken)
        {
            var result = _authService.DecodePayload(jwtToken);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "Decode successfully!"
            ));
        }
    }
}