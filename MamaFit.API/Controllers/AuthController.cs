using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.Token;
using MamaFit.Services.Interface;
using MamaFit.Repositories.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using Microsoft.AspNetCore.Authorization;

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
        
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync();
            return Ok(new ResponseModel<UserReponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                user,
                null,
                "Get current user successfully!"
            ));
        }


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDto model)
        {
            var token = await _authService.SignInAsync(model);
            return Ok(new ResponseModel<TokenResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                token, null,
                "Login successfully!"
            ));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto model)
        {
            await _authService.LogoutAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null, null,
                "Logout successfully!"
            ));
        }
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto model)
        {
            var token = await _authService.RefreshTokenAsync(model);
            return Ok(new ResponseModel<TokenResponseDto>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.CREATED,
                token, null,
                "Refresh token successfully!"
            ));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto model)
        {
            await _authService.VerifyOtpAsync(model);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null, null,
                "OTP verification successful!"
            ));
        }
    }
}
