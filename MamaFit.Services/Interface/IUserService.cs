using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.Services.Interface;

public interface IUserService
{
    Task CompleteRegisterAsync(RegisterUserRequestDto model);
    Task SendRegisterOtpAsync(SendOTPRequestDto model);
    Task<List<UserReponseDto>> GetAllUsersAsync();
    Task<UserReponseDto> GetUserByIdAsync(string userId);
    Task<UserReponseDto> UpdateUserAsync(string userId, UpdateUserRequestDto model);
    Task DeleteUserAsync(string userId);
}