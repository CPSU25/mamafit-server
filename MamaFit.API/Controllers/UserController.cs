using MamaFit.BusinessObjects.DTO.UploadImageDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.Repositories.Helper;
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


    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nameSearch = null,
        [FromQuery] string? roleName = null)
    {
        var pagedUsers = await _service.GetAllUsersAsync(index, pageSize, nameSearch, roleName);
        return Ok(new ResponseModel<PaginatedList<UserReponseDto>>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            pagedUsers, "Get all users successfully!"
        ));
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _service.GetUserByIdAsync(userId);
        return Ok(new ResponseModel<UserReponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            user,
            "Get user by ID successfully!"
        ));
    }

    [HttpPut("profile-picture")]
    public async Task<IActionResult> UpdateUserProfilePicture([FromForm] UploadImageDto model)
    {
        var result = await _service.UpdateUserProfilePictureAsync(model);
        return Ok(new ResponseModel<PhotoUploadResult>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            result,
            "Update user profile picture successfully!"
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
            "Delete user successfully!"
        ));
    }
}