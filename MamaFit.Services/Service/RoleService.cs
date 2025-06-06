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

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<RoleResponseDto>> GetAllRolesAsync(int index = 1, int pageSize = 10, string? nameSearch = null)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var query = repo.Entities.Where(x => x.IsDeleted != true);

        if (!string.IsNullOrWhiteSpace(nameSearch))
        {
            query = query.Where(x => x.RoleName.Contains(nameSearch));
        }

        var resultQuery = await repo.GetPagging(query, index, pageSize);

        var responseItems = resultQuery.Items
            .Select(_mapper.Map<RoleResponseDto>)
            .ToList();

        var responsePaginatedList = new PaginatedList<RoleResponseDto>(
            responseItems,
            resultQuery.TotalCount,
            resultQuery.PageNumber,
            pageSize
        );

        return responsePaginatedList;
    }


    public async Task<RoleResponseDto> GetRoleByIdAsync(string id)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var role = await repo.GetByIdAsync(id);
        if (role == null || role.IsDeleted == true)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "Role is not exist!"
                );
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto model)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        bool exist = repo.Entities.Any(r => r.RoleName == model.RoleName && r.IsDeleted != true);
        if (exist)
            throw new ErrorException(StatusCodes.Status409Conflict,
                ErrorCode.Conflicted, "Role already existed");
        
        var role = _mapper.Map<ApplicationUserRole>(model);
        await repo.InsertAsync(role);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto model)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var role = await repo.GetByIdAsync(id);
        if (role == null || role.IsDeleted)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "Role is not exist!"
            );

        bool exist = repo.Entities.Any(r => r.RoleName == model.RoleName && r.Id != id && r.IsDeleted != true);
        if (exist)
            throw new ErrorException(StatusCodes.Status409Conflict,
                ErrorCode.Conflicted, "Role already existed");

        role.RoleName = model.RoleName;
        
        await repo.UpdateAsync(role);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task DeleteRoleAsync(string id)
    {
        var repo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var role = await repo.GetByIdAsync(id);
        if (role == null || role.IsDeleted)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "Role is not exist!"
            );
        
        await repo.SoftDeleteAsync(role);
        await _unitOfWork.SaveAsync();
    }
}