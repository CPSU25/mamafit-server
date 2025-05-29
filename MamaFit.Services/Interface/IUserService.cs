using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IUserService
{
    Task CompleteRegisterAsync(RegisterUserRequestDto model);
    Task SendRegisterOtpAsync(SendOTPRequestDto model);
    Task ResendOtpAsync(SendOTPRequestDto model);

    Task<PaginatedList<UserReponseDto>> GetAllUsersAsync(
        int index, int pageSize, string? nameSearch, string? roleName);
    Task<UserReponseDto> GetUserByIdAsync(string userId);
    Task<UserReponseDto> UpdateUserAsync(string userId, UpdateUserRequestDto model);
    Task DeleteUserAsync(string userId);
}