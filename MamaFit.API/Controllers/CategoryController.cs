using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var categories = await _categoryService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<CategoryResponseDto>>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                categories,
                null,
                "Get all categories successfully!"
            ));
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetById([FromRoute] string categoryId)
        {
            var category = await _categoryService.GetByIdAsync(categoryId);
            return Ok(new ResponseModel<CategoryResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                category,
                null,
                "Get category successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto requestDto)
        {
            await _categoryService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ResponseCodeConstants.CREATED,
                    null,
                    null,
                    "Created category successfully!"
                ));
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update([FromRoute] string categoryId, [FromBody] CategoryRequestDto requestDto)
        {
            await _categoryService.UpdateAsync(categoryId, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Updated category successfully!"
            ));
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete([FromRoute] string categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Deleted category successfully!"
            ));
        }
    }
}
