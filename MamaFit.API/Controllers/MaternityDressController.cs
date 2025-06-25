using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/maternity-dress")]
    public class MaternityDressController : ControllerBase
    {
        private readonly IMaternityDressService _maternityDressService;

        public MaternityDressController(IMaternityDressService maternityDressService)
        {
            _maternityDressService = maternityDressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var maternityDressList = await _maternityDressService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(ResponseModel<PaginatedList<MaternityDressGetAllResponseDto>>.OkResponseModel(maternityDressList));
        }

        [HttpGet("{maternityDressId}")]
        public async Task<IActionResult> GetById(string maternityDressId)
        {
            var maternityDress = await _maternityDressService.GetByIdAsync(maternityDressId);
            return Ok(ResponseModel<MaternityDressResponseDto>.OkResponseModel(maternityDress));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaternityDressRequestDto requestDto)
        {
            await _maternityDressService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created, ResponseModel<string>.CreatedResponseModel("Created successfully"));
        }

        [HttpPut("{maternityDressId}")]
        public async Task<IActionResult> Update(string maternityDressId, [FromBody] MaternityDressRequestDto requestDto)
        {
            await _maternityDressService.UpdateAsync(maternityDressId, requestDto);
            return Ok(ResponseModel<string>.OkResponseModel("Updated successfully"));
        }

        [HttpDelete("{maternityDressId}")]
        public async Task<IActionResult> Delete(string maternityDressId)
        {
            await _maternityDressService.DeleteAsync(maternityDressId);
            return Ok(ResponseModel<string>.OkResponseModel("Deleted successfully"));
        }
    }
}
