using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.Token;
using MamaFit.Services.Interface;
using MamaFit.Repositories.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MamaFit.BusinessObjects.DTO.OTPDto;
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
        
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var user = await _authService.GetCurrentUserAsync();
                return Ok(ResponseModel<UserReponseDto>.OkResponseModel(user));
            }
            catch (ErrorException ex)
            {
                return StatusCode(
                    ex.StatusCode, 
                    new ResponseModel<object>(
                        ex.StatusCode,
                        ex.ErrorDetail.ErrorCode,
                        ex.ErrorDetail.ErrorMessage?.ToString()
                    )
                );
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }
        
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDto model)
        {
            try
            {
                var token = await _authService.SignInAsync(model);
                return Ok(ResponseModel<TokenResponseDto>.OkResponseModel(token));
            }
            catch (ErrorException ex)
            {
                return StatusCode(
                    ex.StatusCode, 
                    new ResponseModel<object>(
                        ex.StatusCode,
                        ex.ErrorDetail.ErrorCode,
                        ex.ErrorDetail.ErrorMessage?.ToString()
                    )
                );
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto model)
        {
            try
            {
                var token = await _authService.RefreshTokenAsync(model);
                return Ok(ResponseModel<TokenResponseDto>.OkResponseModel(token));
            }
            catch (ErrorException ex)
            {
                return StatusCode(
                    ex.StatusCode, 
                    new ResponseModel<object>(
                        ex.StatusCode,
                        ex.ErrorDetail.ErrorCode,
                        ex.ErrorDetail.ErrorMessage?.ToString()
                    )
                );
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }
        
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto model)
        {
            try
            {
                await _authService.VerifyOtpAsync(model);
                return Ok(ResponseModel<object>.OkResponseModel(null, null, "Xác thực thành công!"));
            }
            catch (ErrorException ex)
            {
                return StatusCode(
                    ex.StatusCode, 
                    new ResponseModel<object>(
                        ex.StatusCode,
                        ex.ErrorDetail.ErrorCode,
                        ex.ErrorDetail.ErrorMessage?.ToString()
                    )
                );
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }

    }
}
