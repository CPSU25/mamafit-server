using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/component-option")]
    public class ComponentOptionController : ControllerBase
    {
        private readonly IComponentOptionService _componentOptionService;

        public ComponentOptionController(IComponentOptionService componentOptionService)
        {
            _componentOptionService = componentOptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var options = await _componentOptionService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<ComponentOptionResponseDto>>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                options,
                null,
                "Get all component options successfully!"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var option = await _componentOptionService.GetByIdAsync(id);
            return Ok(new ResponseModel<ComponentOptionResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                option,
                null,
                "Get component option successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComponentOptionRequestDto requestDto)
        {
            await _componentOptionService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ResponseCodeConstants.CREATED,
                    null,
                    null,
                    "Created component option successfully!"
                ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ComponentOptionRequestDto requestDto)
        {
            await _componentOptionService.UpdateAsync(id, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Updated component option successfully!"
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _componentOptionService.DeleteAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Deleted component option successfully!"
            ));
        }
    }
}
