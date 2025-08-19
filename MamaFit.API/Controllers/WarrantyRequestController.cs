using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("original/order/{orderId}")]
        public async Task<IActionResult> GetOriginalOrders(string orderId)
        {
            var result = await _warrantyRequestService.DetailsByIdAsync(orderId);
            return Ok(new ResponseModel<WarrantyDetailResponseDto>(
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
        
            return Ok(new ResponseModel<WarrantyGetByIdResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "Get warranty request by id successful"
            ));
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WarrantyRequestCreateDto warrantyRequestCreateDto,
            [FromHeader(Name = "Authorization")] string accessToken)
        {
            var id = await _warrantyRequestService.CreateAsync(warrantyRequestCreateDto, accessToken);

            return Ok(
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.SUCCESS,
                    id,
                    "Warranty request created successfully"
                ));
        }
        
        [Authorize(Roles = "BranchManager")]
        [HttpPost("branch-manager")] 
        public async Task<IActionResult> CreateRequestInBranch([FromBody] WarrantyBranchRequestDto dto,
            [FromHeader(Name = "Authorization")] string accessToken)
        {
            var id = await _warrantyRequestService.CreateRequestInBranch(dto, accessToken);

            return Ok(
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.SUCCESS,
                    id,
                    "Warranty request created successfully by manager"
                ));
        }
        [Authorize(Roles = "BranchManager")]
        [HttpPut("complete-order/{orderId}")]
        public async Task<IActionResult> CompleteWarrantyOrder(string orderId)
        {
            await _warrantyRequestService.CompleteWarrantyOrderAsync(orderId);
        
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Warranty order completed successfully"
            ));
        }
        [Authorize(Roles = "BranchManager,Admin,Manager")]
        
        [HttpPut("complete-warranty-request/{warrantyRequestId}")]
        public async Task<IActionResult> CompleteWarrantyRequest(string warrantyRequestId)
        {
            await _warrantyRequestService.CompleteWarrantyRequestAsync(warrantyRequestId);
        
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Warranty request completed successfully"
            ));
        }
        [HttpPost("ship-paid/{warrantyRequestId}")]
        public async Task<IActionResult> ShipPaidWarranty(string warrantyRequestId)
        {
            var result = await _warrantyRequestService.ShipPaidWarrantyAsync(warrantyRequestId);
            return Ok(new ResponseModel<WarrantyDecisionResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "Warranty request shipped successfully"
            ));
        }
        
        [HttpPost("decisions/{warrantyRequestId}")]
        public async Task<IActionResult> Decide(string warrantyRequestId, [FromBody] WarrantyDecisionRequestDto dto)
        {
            var result = await _warrantyRequestService.DecideAsync(warrantyRequestId, dto);
        
            return Ok(new ResponseModel<WarrantyDecisionResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                result,
                "Warranty decision processed successfully"
            ));
        }
        // [HttpPut]
        // public async Task<IActionResult> Update(string id, [FromBody] WarrantyRequestUpdateDto warrantyRequestUpdateDto)
        // {
        //     await _warrantyRequestService.UpdateAsync(id, warrantyRequestUpdateDto);
        //
        //     return Ok(
        //         new ResponseModel<string>(
        //             StatusCodes.Status200OK,
        //             ApiCodes.SUCCESS,
        //             null,
        //             "Warranty request updated successfully"
        //         ));
        // }

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
