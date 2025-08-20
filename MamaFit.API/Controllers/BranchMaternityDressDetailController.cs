using MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/branch-maternity-dress-detail")]
public class BranchMaternityDressDetailController : ControllerBase
{
    private readonly IBranchMaternityDressDetailService _branchMaternityDressDetailService;

    public BranchMaternityDressDetailController(IBranchMaternityDressDetailService branchMaternityDressDetailService)
    {
        _branchMaternityDressDetailService = branchMaternityDressDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = "Authorization")] string accessToken,
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var dressDetails = await _branchMaternityDressDetailService.GetAllAsync(index, pageSize, accessToken, search);
        return Ok(new ResponseModel<PaginatedList<GetDetailById>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            dressDetails,
            "Get all branch maternity dress details successfully!"
        ));
    }

    [HttpGet("{branchId}/{dressId}")]
    public async Task<IActionResult> GetById([FromRoute] string branchId, [FromRoute] string dressId)
    {
        var dressDetail = await _branchMaternityDressDetailService.GetByIdAsync(branchId, dressId);
        return Ok(new ResponseModel<GetDetailById>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            dressDetail,
            "Get branch maternity dress detail successfully!"
        ));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BranchMaternityDressDetailDto request)
    {
        var createdDressDetail = await _branchMaternityDressDetailService.CreateAsync(request);
        return StatusCode(StatusCodes.Status201Created, new ResponseModel<BranchMaternityDressDetailDto>(
            StatusCodes.Status201Created,
            ApiCodes.CREATED,
            createdDressDetail,
            "Created branch maternity dress detail successfully!"
        ));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BranchMaternityDressDetailDto request)
    {
        var updatedDressDetail = await _branchMaternityDressDetailService.UpdateAsync(request);
        return Ok(new ResponseModel<BranchMaternityDressDetailDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            updatedDressDetail,
            "Updated branch maternity dress detail successfully!"
        ));
    }

    [HttpDelete("{branchId}/{dressId}")]
    public async Task<IActionResult> Delete([FromRoute] string branchId, [FromRoute] string dressId)
    {
        await _branchMaternityDressDetailService.DeleteAsync(branchId, dressId);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted branch maternity dress detail successfully!"
        ));
    }
}