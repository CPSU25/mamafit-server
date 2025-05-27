using System.Security.Claims;
using AutoMapper;
using CloudinaryDotNet.Actions;
using MamaFit.BusinessObjects.DTO.Role;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    private string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
    }
    
    public async Task<List<RoleResponseDto>> GetAllRolesAsync()
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var roles = await repo.GetAllAsync();
        var result = roles.Where(x => x.IsDeleted != true).Select(_mapper.Map<RoleResponseDto>).ToList();
        return result;
    }

    public async Task<RoleResponseDto> GetRoleByIdAsync(string id)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var role = await repo.GetByIdAsync(id);
        if (role == null || role.IsDeleted == true)
            throw new ErrorException(404, "not_found", "Role không tồn tại");
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto model)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        bool exist = repo.Entities.Any(r => r.RoleName == model.RoleName && r.IsDeleted != true);
        if (exist)
            throw new ErrorException(400, "duplicate", "Role đã tồn tại");

        var now = DateTime.UtcNow;
        var role = new ApplicationUserRole()
        {
            RoleName = model.RoleName,
            CreatedAt = now,
            CreatedBy = GetCurrentUserName(),
            UpdatedAt = now,
            IsDeleted = false
        };
        await repo.InsertAsync(role);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto model)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var role = await repo.GetByIdAsync(id);
        if (role == null || role.IsDeleted == true)
            throw new ErrorException(404, "not_found", "Role không tồn tại");

        bool exist = repo.Entities.Any(r => r.RoleName == model.RoleName && r.Id != id && r.IsDeleted != true);
        if (exist)
            throw new ErrorException(400, "duplicate", "Role đã tồn tại");

        role.RoleName = model.RoleName;
        role.UpdatedAt = DateTime.UtcNow;
        role.UpdatedBy = GetCurrentUserName();
        
        await repo.UpdateAsync(role);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task DeleteRoleAsync(string id)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var role = await repo.GetByIdAsync(id);
        if (role == null || role.IsDeleted == true)
            throw new ErrorException(404, "not_found", "Role không tồn tại");

        role.IsDeleted = true;
        role.UpdatedAt = DateTime.UtcNow;
        role.UpdatedBy = GetCurrentUserName();
        
        await repo.UpdateAsync(role);
        await _unitOfWork.SaveAsync();
    }
}