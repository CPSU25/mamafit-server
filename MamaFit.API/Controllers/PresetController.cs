using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/preset")]
    public class PresetController : ControllerBase
    {
        private readonly IPresetService _presetService;

        public PresetController(IPresetService presetService)
        {
            _presetService = presetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int index = 1, int pageSize = 10, string? search = null, EntitySortBy? sortBy = null)
        {
            var presets = await _presetService.GetAll(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<PresetGetAllResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                presets,
                "Get all presets successfully!"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var preset = await _presetService.GetById(id);
            return Ok(new ResponseModel<PresetGetByIdResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                preset,
                "Get preset successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PresetCreateRequestDto requestDto)
        {
            await _presetService.CreatePresetAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.CREATED,
                    null,
                    "Created preset successfully!"
                ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] PresetUpdateRequestDto requestDto)
        {
            await _presetService.UpdatePresetAsync(id, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Updated preset successfully!"
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _presetService.DeletePresetAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted preset successfully!"
            ));
        }
    }
}
