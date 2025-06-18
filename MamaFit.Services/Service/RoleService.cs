using AutoMapper;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<PaginatedList<RoleResponseDto>> GetAllRolesAsync(int index = 1, int pageSize = 10, string? nameSearch = null)
    {
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

        return responsePaginatedList;
    }


    public async Task<RoleResponseDto> GetRoleByIdAsync(string id)
    {
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
        if (role == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "Role is not exist!"
                );
        return _mapper.Map<RoleResponseDto>(role);
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
        return _mapper.Map<RoleResponseDto>(role);
    }
    
    public async Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto model)
    {
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
    }
}