using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/branch")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var branches = await _branchService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<BranchResponseDto>>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                branches,
                "Get all branches successfully!"
            ));
        }

        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetById([FromRoute] string branchId)
        {
            var branch = await _branchService.GetByIdAsync(branchId);
            return Ok(new ResponseModel<BranchResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                branch,
                "Get branch successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchCreateDto requestDto)
        {
            await _branchService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ResponseCodeConstants.CREATED,
                    null,
                    "Created branch successfully!"
                ));
        }

        [HttpPut("{branchId}")]
        public async Task<IActionResult> Update([FromRoute] string branchId, [FromBody] BranchCreateDto requestDto)
        {
            await _branchService.UpdateAsync(branchId, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                "Updated branch successfully!"
            ));
        }

        [HttpDelete("{branchId}")]
        public async Task<IActionResult> Delete([FromRoute] string branchId)
        {
            await _branchService.DeleteAsync(branchId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                "Deleted branch successfully!"
            ));
        }
    }
}

