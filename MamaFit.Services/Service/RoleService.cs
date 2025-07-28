using System.Diagnostics;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly ICacheService _cache;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _cache = cache;
    }
    
    public async Task<PaginatedList<RoleResponseDto>> GetAllRolesAsync(int index = 1, int pageSize = 10, string? nameSearch = null)
    {
        int version = await _cache.GetVersionAsync("roles");
        string cacheKey = $"roles:v{version}:{index}:{pageSize}:{nameSearch ?? ""}";
        
        var cached = await _cache.GetAsync<PaginatedList<RoleResponseDto>>(cacheKey);
        if (cached != null)
            return cached;
        
        var roles = await _unitOfWork.RoleRepository.GetRolesAsync(index, pageSize, nameSearch);
        var responseItems = roles.Items
            .Select(role => _mapper.Map<RoleResponseDto>(role))
            .ToList();

        var responsePaginatedList = new PaginatedList<RoleResponseDto>(
            responseItems,
            roles.TotalCount,
            roles.PageNumber,
            pageSize
        );
        
        await _cache.SetAsync(cacheKey, responsePaginatedList, TimeSpan.FromMinutes(15));
        return responsePaginatedList;
    }


    public async Task<RoleResponseDto> GetRoleByIdAsync(string id)
    {
        string cacheKey = $"role:{id}";
        var cached = await _cache.GetAsync<RoleResponseDto>(cacheKey);
        if (cached != null)
            return cached;
        
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (role == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "Role is not exist!"
                );
        
        var mapped = _mapper.Map<RoleResponseDto>(role);
        await _cache.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(15));
        return mapped;
    }
    
    public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var exist = await _unitOfWork.RoleRepository.IsRoleNameExistedAsync(model.RoleName);
        if (exist)
            throw new ErrorException(StatusCodes.Status409Conflict,
                ApiCodes.CONFLICT, "Role already existed");
        
        var role = _mapper.Map<ApplicationUserRole>(model);
        await _unitOfWork.RoleRepository.CreateAsync(role);
        await _unitOfWork.SaveChangesAsync();
        await _cache.IncreaseVersionAsync("roles");
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (role == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "Role is not exist!"
            );

        bool exist = await _unitOfWork.RoleRepository.IsRoleNameExistedAsync(model.RoleName);
        if (exist)
            throw new ErrorException(StatusCodes.Status409Conflict,
                ApiCodes.CONFLICT, "Role already existed");

        role.RoleName = model.RoleName;

        await _unitOfWork.RoleRepository.UpdateRoleAsync(role);
        await _unitOfWork.SaveChangesAsync();
        await _cache.IncreaseVersionAsync("roles");
        await _cache.RemoveAsync($"role:{id}");
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task DeleteRoleAsync(string id)
    {
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (role == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "Role is not exist!"
            );
        
        await _unitOfWork.RoleRepository.DeleteAsync(role);
        await _unitOfWork.SaveChangesAsync();
        await _cache.IncreaseVersionAsync("roles");
        await _cache.RemoveAsync($"role:{id}");
    }
}