using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/style")]
    public class StyleController : ControllerBase
    {
        private readonly IStyleService _styleService;

        public StyleController(IStyleService styleService)
        {
            _styleService = styleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var styles = await _styleService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<StyleResponseDto>>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                styles,
                null,
                "Get all styles successfully!"
            ));
        }

        [HttpGet("{styleId}")]
        public async Task<IActionResult> GetById([FromRoute] string styleId)
        {
            var style = await _styleService.GetByIdAsync(styleId);
            return Ok(new ResponseModel<StyleResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                style,
                null,
                "Get style successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StyleRequestDto requestDto)
        {
            await _styleService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ResponseCodeConstants.CREATED,
                    null,
                    null,
                    "Created style successfully!"
                ));
        }

        [HttpPut("{styleId}")]
        public async Task<IActionResult> Update([FromRoute] string styleId, [FromBody] StyleRequestDto requestDto)
        {
            await _styleService.UpdateAsync(styleId, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Updated style successfully!"
            ));
        }

        [HttpDelete("{styleId}")]
        public async Task<IActionResult> Delete([FromRoute] string styleId)
        {
            await _styleService.DeleteAsync(styleId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Deleted style successfully!"
            ));
        }
    }
}
