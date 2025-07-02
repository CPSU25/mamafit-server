using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/warranty-request")]
    public class WarrantyRequestController : ControllerBase
    {
        private readonly IWarrantyRequestService _warrantyRequestService;

        public WarrantyRequestController(IWarrantyRequestService warrantyRequestService)
        {
            _warrantyRequestService = warrantyRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] EntitySortBy? sortBy = null)
        {
            var result = await _warrantyRequestService.GetAllWarrantyRequestAsync(index, pageSize, search, sortBy);

            return Ok(new ResponseModel<PaginatedList<WarrantyRequestGetAllDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "Get all warranty successful"
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _warrantyRequestService.GetWarrantyRequestByIdAsync(id);

            return Ok(new ResponseModel<WarrantyRequestGetByIdDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "Get warranty request by id successful"
            ));
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] WarrantyRequestCreateDto warrantyRequestCreateDto)
        //{
        //    await _warrantyRequestService.CreateAsync(warrantyRequestCreateDto);

        //    return Ok(
        //        new ResponseModel<string>(
        //            StatusCodes.Status201Created,
        //            ApiCodes.SUCCESS,
        //            null,
        //            "Warranty request created successfully"
        //        ));
        //}

        [HttpPut]
        public async Task<IActionResult> Update(string id, [FromBody] WarrantyRequestUpdateDto warrantyRequestUpdateDto)
        {
            await _warrantyRequestService.UpdateAsync(id, warrantyRequestUpdateDto);

            return Ok(
                new ResponseModel<string>(
                    StatusCodes.Status200OK,
                    ApiCodes.SUCCESS,
                    null,
                    "Warranty request updated successfully"
                ));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _warrantyRequestService.DeleteAsync(id);

            return Ok(
                new ResponseModel<string>(
                    StatusCodes.Status200OK,
                    ApiCodes.SUCCESS,
                    null,
                    "Warranty request deleted successfully"
                ));
        }
    }
}
