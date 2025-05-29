
using MamaFit.BusinessObjects.DTO.MaternityDressDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/maternity-dress")]
    public class MaternityDressController : ControllerBase
    {
        private readonly IMaternityDressService _maternityDressService;

        public MaternityDressController(IMaternityDressService maternityDressService)
        {
            _maternityDressService = maternityDressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var maternityDressList = await _maternityDressService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<MaternityDressResponseDto>>
                (
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                maternityDressList
                ));
        }

        [HttpGet("{maternityDressId}")]
        [ProducesResponseType(typeof(ResponseModel<MaternityDressResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] string maternityDressId)
        {
            try
            {
                var maternityDress = await _maternityDressService.GetByIdAsync(maternityDressId);
                return Ok(ResponseModel<MaternityDressResponseDto>.OkResponseModel(maternityDress));
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
        public async Task<IActionResult> Create([FromBody] MaternityDressRequestDto requestDto)
        {
            try
            {
                await _maternityDressService.CreateAsync(requestDto);
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

        [HttpPut("{maternityDressId}")]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string maternityDressId, [FromBody] MaternityDressRequestDto requestDto)
        {
            try
            {
                await _maternityDressService.UpdateAsync(maternityDressId, requestDto);
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

        [HttpDelete("{maternityDressId}")]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string maternityDressId)
        {
            try
            {
                await _maternityDressService.DeleteAsync(maternityDressId);
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
