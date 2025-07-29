using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/milestone")]
    public class MilestoneController : ControllerBase
    {
        private readonly IMilestoneService _milestoneService;

        public MilestoneController(IMilestoneService milestoneService)
        {
            _milestoneService = milestoneService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pagesize = 1,
            [FromQuery] string? search = null,
            [FromQuery] EntitySortBy? sortBy = EntitySortBy.CREATED_AT_DESC)
        {
            await _milestoneService.GetAllAsync(index, pagesize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<MilestoneResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                await _milestoneService.GetAllAsync(index, pagesize, search, sortBy),
                "Get all milestones successfully!"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var milestone = await _milestoneService.GetByIdAsync(id);
            return Ok(new ResponseModel<MilestoneGetByIdResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                milestone,
                "Get milestone successfully!"
            ));
        }

        [HttpGet("order-item/{orderItemId}")]
        public async Task<IActionResult> GetMilestoneByOrderItemId([FromRoute] string orderItemId)
        {
            var milestones = await _milestoneService.GetMilestoneByOrderItemId(orderItemId);
            return Ok(new ResponseModel<List<MilestoneAchiveOrderItemResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                milestones,
                "Get milestones by order item successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MilestoneRequestDto request)
        {
            await _milestoneService.CreateAsync(request);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.CREATED,
                    null,
                    "Created milestone successfully!"
                ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] MilestoneRequestDto request)
        {
            await _milestoneService.UpdateAsync(id, request);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Updated milestone successfully!"
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _milestoneService.DeleteAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted milestone successfully!"
            ));
        }
    }
}
