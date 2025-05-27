using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }
    
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOTPRequestDto model)
    {
        try
        {
            await _service.SendRegisterOtpAsync(model);
            return Ok(ResponseModel<object>.OkResponseModel(null, null, "Đã gửi OTP thành công!"));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode, new ResponseModel<object>(
                ex.StatusCode,
                ex.ErrorDetail.ErrorCode,
                ex.ErrorDetail.ErrorMessage?.ToString()
            ));
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
        }
    }
    

    [HttpPost("complete-register")]
    public async Task<IActionResult> CompleteRegister([FromBody] RegisterUserRequestDto model)
    {
        try
        {
            await _service.CompleteRegisterAsync(model);
            return Ok(ResponseModel<object>.OkResponseModel(null, null, "Đăng ký tài khoản thành công!"));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode, new ResponseModel<object>(
                ex.StatusCode,
                ex.ErrorDetail.ErrorCode,
                ex.ErrorDetail.ErrorMessage?.ToString()
            ));
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(ResponseModel<List<UserReponseDto>>.OkResponseModel(users));
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
        }
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            var user = await _service.GetUserByIdAsync(userId);
            return Ok(ResponseModel<UserReponseDto>.OkResponseModel(user));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode, new ResponseModel<object>(
                ex.StatusCode,
                ex.ErrorDetail.ErrorCode,
                ex.ErrorDetail.ErrorMessage?.ToString()
            ));
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
        }
    }
}
