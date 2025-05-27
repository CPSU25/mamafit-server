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
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(ResponseModel<List<RoleResponseDto>>.OkResponseModel(roles));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode,
                new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode,
                    ex.ErrorDetail.ErrorMessage?.ToString()));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return Ok(ResponseModel<RoleResponseDto>.OkResponseModel(role));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode,
                new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode,
                    ex.ErrorDetail.ErrorMessage?.ToString()));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleRequestDto model)
    {
        try
        {
            var role = await _roleService.CreateRoleAsync(model);
            return Ok(ResponseModel<RoleResponseDto>.OkResponseModel(role));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode,
                new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode,
                    ex.ErrorDetail.ErrorMessage?.ToString()));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] RoleRequestDto model)
    {
        try
        {
            var role = await _roleService.UpdateRoleAsync(id, model);
            return Ok(ResponseModel<RoleResponseDto>.OkResponseModel(role));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode,
                new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode,
                    ex.ErrorDetail.ErrorMessage?.ToString()));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok(ResponseModel<object>.OkResponseModel(null, null, "Xóa role thành công!"));
        }
        catch (ErrorException ex)
        {
            return StatusCode(ex.StatusCode,
                new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode,
                    ex.ErrorDetail.ErrorMessage?.ToString()));
        }
    }
}