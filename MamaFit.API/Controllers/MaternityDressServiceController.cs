using MamaFit.BusinessObjects.DTO.MaternityDressServiceDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/maternity-dress-service")]
public class MaternityDressServiceController : ControllerBase
{
    private readonly IMaternityDressServiceService _service;
    
    public MaternityDressServiceController(IMaternityDressServiceService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] EntitySortBy? sortBy = null)
    {
        var maternityDressServices = await _service.GetAllAsync(index, pageSize, search, sortBy);
        return Ok(new ResponseModel<PaginatedList<MaternityDressServiceDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            maternityDressServices,
            "Get all maternity dress services successfully!"
        ));
    }
}