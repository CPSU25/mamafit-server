using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/maternity-dress-detail")]
    public class MaternityDressDetailController : ControllerBase
    {
        private readonly IMaternityDressDetailService _detailService;

        public MaternityDressDetailController(IMaternityDressDetailService detailService)
        {
            _detailService = detailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var result = await _detailService.GetAllAsync(index, pageSize, search, sortBy);

            return Ok(new ResponseModel<PaginatedList<MaternityDressDetailResponseDto>>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                result,
                "Get all maternity dress details successfully!"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _detailService.GetByIdAsync(id);

            return Ok(new ResponseModel<MaternityDressDetailResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                result,
                "Get maternity dress detail successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaternityDressDetailRequestDto requestDto)
        {
            await _detailService.CreateAsync(requestDto);

            return StatusCode(StatusCodes.Status201Created, new ResponseModel<string>(
                StatusCodes.Status201Created,
                ResponseCodeConstants.CREATED,
                null,
                "Created maternity dress detail successfully!"
            ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] MaternityDressDetailRequestDto requestDto)
        {
            await _detailService.UpdateAsync(id, requestDto);

            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                "Updated maternity dress detail successfully!"
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _detailService.DeleteAsync(id);

            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                "Deleted maternity dress detail successfully!"
            ));
        }
    }
}
