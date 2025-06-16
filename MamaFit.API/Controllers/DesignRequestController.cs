using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/design-request")]
    public class DesignRequestController : ControllerBase
    {
        private readonly IDesignRequestService _designRequestService;

        public DesignRequestController(IDesignRequestService designRequestService)
        {
            _designRequestService = designRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var requests = await _designRequestService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<DesignResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                requests,
                "Get all design requests successfully!"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var request = await _designRequestService.GetByIdAsync(id);
            return Ok(new ResponseModel<DesignResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                request,
                "Get design request successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DesignRequestCreateDto requestDto)
        {
            await _designRequestService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.CREATED,
                    null,
                    "Created design request successfully!"
                ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _designRequestService.DeleteAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted design request successfully!"
            ));
        }
    }
}
