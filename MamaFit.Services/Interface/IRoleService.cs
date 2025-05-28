using MamaFit.BusinessObjects.DTO.Role;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IRoleService
{
    Task<PaginatedList<RoleResponseDto>> GetAllRolesAsync(int index , int pageSize , string? nameSearch);
    Task<RoleResponseDto> GetRoleByIdAsync(string id);
    Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto model);
    Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto model);
    Task DeleteRoleAsync(string id);
}