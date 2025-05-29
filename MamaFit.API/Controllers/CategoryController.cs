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
                categories
            ));
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(ResponseModel<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] string categoryId)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(categoryId);
                return Ok(ResponseModel<CategoryResponseDto>.OkResponseModel(category));
            }
            catch (ErrorException ex)
            {
                return StatusCode(ex.StatusCode, new ResponseModel<object>(
                    ex.StatusCode,
                    ex.ErrorDetail.ErrorCode,
                    ex.ErrorDetail.ErrorMessage
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto requestDto)
        {
            try
            {
                await _categoryService.CreateAsync(requestDto);
                return StatusCode(StatusCodes.Status201Created,
                    ResponseModel<string>.CreatedResponseModel("Created successfully"));
            }
            catch (ErrorException ex)
            {
                return StatusCode(ex.StatusCode,
                    new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode, ex.ErrorDetail.ErrorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string categoryId, [FromBody] CategoryRequestDto requestDto)
        {
            try
            {
                await _categoryService.UpdateAsync(categoryId, requestDto);
                return Ok(ResponseModel<string>.OkResponseModel("Updated successfully"));
            }
            catch (ErrorException ex)
            {
                return StatusCode(ex.StatusCode,
                    new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode, ex.ErrorDetail.ErrorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string categoryId)
        {
            try
            {
                await _categoryService.DeleteAsync(categoryId);
                return Ok(ResponseModel<string>.OkResponseModel("Deleted successfully"));
            }
            catch (ErrorException ex)
            {
                return StatusCode(ex.StatusCode,
                    new ResponseModel<object>(ex.StatusCode, ex.ErrorDetail.ErrorCode, ex.ErrorDetail.ErrorMessage));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ResponseModel<object>.InternalErrorResponseModel(null, null, ex.Message));
            }
        }
    }
}
