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
        await _service.SendRegisterOtpAsync(model);
        return Ok(new ResponseModel<object>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null,
            null,
            "Send OTP successfully!"
        ));
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtpAsync([FromBody] SendOTPRequestDto model)
    {
        await _service.ResendOtpAsync(model);
        return Ok(new ResponseModel<object>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null,
            null,
            "Resend OTP successfully!"
        ));
    }

    [HttpPost("complete-register")]
    public async Task<IActionResult> CompleteRegister([FromBody] RegisterUserRequestDto model)
    {
        await _service.CompleteRegisterAsync(model);
        return Ok(new ResponseModel<object>(
            StatusCodes.Status201Created,
            ResponseCodeConstants.CREATED,
            null,
            null,
            "Register user successfully!"));
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
        var user = await _service.GetUserByIdAsync(userId);
        return Ok(new ResponseModel<object>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null,
            null,
            "Resend OTP successfully!"
        ));
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequestDto model)
    {
            var updatedUser = await _service.UpdateUserAsync(userId, model);
            return Ok(new ResponseModel<UserReponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                updatedUser,
                null,
                "Update user successfully!"
            ));
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
            await _service.DeleteUserAsync(userId);
            return Ok(new ResponseModel<object>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Delete user successfully!"
            ));
    }
}