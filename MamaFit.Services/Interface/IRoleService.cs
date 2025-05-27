using MamaFit.BusinessObjects.DTO.Role;
using MamaFit.BusinessObjects.DTO.RoleDto;

namespace MamaFit.Services.Interface;

public interface IRoleService
{
    Task<List<RoleResponseDto>> GetAllRolesAsync();
    Task<RoleResponseDto> GetRoleByIdAsync(string id);
    Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto model);
    Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto model);
    Task DeleteRoleAsync(string id);
}