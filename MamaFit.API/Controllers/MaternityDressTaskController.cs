using MamaFit.BusinessObjects.DTO.MaternityDressTask;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class MaternityDressTaskController : ControllerBase
    {
        private readonly IMaternityDressTaskService _maternityDressTaskService;

        public MaternityDressTaskController(IMaternityDressTaskService maternityDressTaskService)
        {
            _maternityDressTaskService = maternityDressTaskService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] EntitySortBy? sortBy = EntitySortBy.CREATED_AT_DESC)
        {
            var responseList = await _maternityDressTaskService.GetAllAsync(index, pageSize, search, sortBy);

            return Ok(new ResponseModel<PaginatedList<MaternityDressTaskResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                responseList,
                "Get all maternity dress tasks successfully!"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute] string id)
        {
            var task = await _maternityDressTaskService.GetById(id);
            return Ok(new ResponseModel<MaternityDressTaskResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                task,
                "Get maternity dress task successfully!"
            ));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MaternityDressTaskRequestDto request)
        {
            await _maternityDressTaskService.CreateAsync(request);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.CREATED,
                    null,
                    "Created maternity dress task successfully!"
                ));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] MaternityDressTaskRequestDto request)
        {
            await _maternityDressTaskService.UpdateAsync(id, request);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Updated maternity dress task successfully!"
            ));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            await _maternityDressTaskService.DeleteAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted maternity dress task successfully!"
            ));
        }
    }
}
