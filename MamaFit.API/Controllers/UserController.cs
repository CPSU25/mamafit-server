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
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nameSearch = null,
        [FromQuery] string? roleId = null)
    {
        var pagedUsers = await _service.GetAllUsersAsync(index, pageSize, nameSearch, roleId);
        return Ok(new ResponseModel<PaginatedList<UserReponseDto>>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            pagedUsers
        ));
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
    
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequestDto model)
    {
        try
        {
            var updatedUser = await _service.UpdateUserAsync(userId, model);
            return Ok(ResponseModel<UserReponseDto>.OkResponseModel(updatedUser));
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
    
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            await _service.DeleteUserAsync(userId);
            return Ok(ResponseModel<object>.OkResponseModel(null, null, "Xóa người dùng thành công!"));
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
