using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/component")]
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ComponentController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var components = await _componentService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<ComponentResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                components,
                "Get all components successfully!"
            ));
        }

        [HttpGet("{componentId}")]
        public async Task<IActionResult> GetById([FromRoute] string componentId)
        {
            var component = await _componentService.GetByIdAsync(componentId);
            return Ok(new ResponseModel<ComponentGetByIdResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                component,
                "Get component successfully!"
                ));
        }

        [HttpGet("preset/style/{styleId}")]
        public async Task<IActionResult> GetAllByStyleId([FromRoute] string styleId)
        {
            var components = await _componentService.GetComponentHavePresetByStyleId(styleId);
            return Ok(new ResponseModel<List<ComponentGetByIdResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                components,
                "Get all components by style ID successfully!"
                ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComponentRequestDto requestDto)
        {
            await _componentService.CreateAsync(requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status201Created,
                ApiCodes.SUCCESS,
                "Component created successfully",
                "Created successfully"
            ));
        }

        [HttpPut("{componentId}")]
        public async Task<IActionResult> Update(string componentId, [FromBody] ComponentRequestDto requestDto)
        {
            await _componentService.UpdateAsync(componentId, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                "Component updated successfully",
                "Updated successfully"
            ));
        }

        [HttpDelete("{componentId}")]
        public async Task<IActionResult> Delete(string componentId)
        {
            await _componentService.DeleteAsync(componentId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                "Component deleted successfully",
                "Deleted successfully"
            ));
        }
    }
}
