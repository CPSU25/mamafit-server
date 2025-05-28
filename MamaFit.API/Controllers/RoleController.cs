using MamaFit.BusinessObjects.DTO.Role;
using MamaFit.BusinessObjects.DTO.RoleDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/role")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nameSearch = null)
    {
        var pagedRoles = await _roleService.GetAllRolesAsync(index, pageSize, nameSearch);
        return Ok(new ResponseModel<PaginatedList<RoleResponseDto>>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            pagedRoles
        ));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        return Ok(new ResponseModel<RoleResponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            role, null, 
            "Get role by ID successfully!"
    ));
}

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleRequestDto model)
    {
        var role = await _roleService.CreateRoleAsync(model);
        return Ok(new ResponseModel<RoleResponseDto>(
            StatusCodes.Status201Created,
            ResponseCodeConstants.CREATED,
            role, null,
            "Create role successfully!"
        ));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] RoleRequestDto model)
    {
        var role = await _roleService.UpdateRoleAsync(id, model);
        return Ok(new ResponseModel<RoleResponseDto>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            role, null,
            "Update role successfully!"
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _roleService.DeleteRoleAsync(id);
        return Ok(new ResponseModel<object>(
            StatusCodes.Status200OK,
            ResponseCodeConstants.SUCCESS,
            null, null,
            "Delete role successfully!"
        ));
    }
}