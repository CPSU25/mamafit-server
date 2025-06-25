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
                ApiCodes.SUCCESS,
                styles,
                "Get all styles successfully!"
            ));
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetAllByCategory(
            string categoryId,
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var styles = await _styleService.GetAllByCategoryAsync(categoryId, index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<StyleResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                styles,
                "Get all styles successfully!"
            ));
        }

        [HttpGet("{styleId}")]
        public async Task<IActionResult> GetById([FromRoute] string styleId)
        {
            var style = await _styleService.GetByIdAsync(styleId);
            return Ok(new ResponseModel<StyleGetByIdResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                style,
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
                    ApiCodes.CREATED,
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
                ApiCodes.SUCCESS,
                null,
                "Updated style successfully!"
            ));
        }

        [HttpPut]
        [Route("assign-components")]
        public async Task<IActionResult> AssignComponentsToStyle([FromBody] StyleAssignComponentRequestDto request)
        {
            await _styleService.AssignComponentToStyle(request.StyleId, request.ComponentIds);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Assigned components to style successfully!"
            ));
        }

        [HttpDelete("{styleId}")]
        public async Task<IActionResult> Delete([FromRoute] string styleId)
        {
            await _styleService.DeleteAsync(styleId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted style successfully!"
            ));
        }
    }
}
