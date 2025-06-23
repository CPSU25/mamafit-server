using MamaFit.BusinessObjects.DTO.MaternityDressCustomizationDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/custom")]
    public class MaternityDressCustomizationController : ControllerBase
    {
        private readonly IMaternityDressCustomizationService _maternityDressCustomizationService;

        public MaternityDressCustomizationController(IMaternityDressCustomizationService maternityDressCustomizationService)
        {
            _maternityDressCustomizationService = maternityDressCustomizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomizations(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] EntitySortBy? sortBy = EntitySortBy.CREATED_AT_DESC)
        {
            var responseList = await _maternityDressCustomizationService.GetAll(index, pageSize, search, sortBy);

            return Ok(new ResponseModel<PaginatedList<CustomResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                responseList,
                "Get all customizations successfully!"
            ));
        }

        [HttpGet]
        [Route("{customizationId}")]
        public async Task<IActionResult> GetCustomizationById([FromRoute] string customizationId)
        {
            var response = await _maternityDressCustomizationService.GetById(customizationId);

            return Ok(new ResponseModel<CustomResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                response,
                "Get customization successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomization([FromBody] CustomCreateRequestDto request)
        {
            await _maternityDressCustomizationService.CreateCustom(request);

            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.CREATED,
                    null,
                    "Created customization successfully!"
                ));
        }

        [HttpPut("{customizationId}")]
        public async Task<IActionResult> UpdateCustomization(string customizationId, [FromBody] CustomUpdateRequestDto request)
        {
            await _maternityDressCustomizationService.UpdateCustom(customizationId, request);

            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Updated customization successfully!"
            ));
        }

        [HttpDelete("{customizationId}")]
        public async Task<IActionResult> DeleteCustomization(string customizationId)
        {
            await _maternityDressCustomizationService.DeleteCustom(customizationId);

            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted customization successfully!"
            ));
        }
    }
}
