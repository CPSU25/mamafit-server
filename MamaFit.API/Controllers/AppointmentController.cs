using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] AppointmentOrderBy? sortBy = AppointmentOrderBy.CREATED_AT_DESC)
        {
            var appointments = await _appointmentService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<AppointmentResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                appointments,
                "Get all appointments successfully!"
            ));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] AppointmentOrderBy? sortBy = AppointmentOrderBy.CREATED_AT_DESC)
        {
            var appointments = await _appointmentService.GetByUserId(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<AppointmentResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                appointments,
                "Get all appointments by user successfully!"
            ));
        }

        [HttpGet("slot")]
        public async Task<IActionResult> GetSlot(
            [FromQuery] string branchId,
            [FromQuery] DateOnly date)
        {
            var slots = await _appointmentService.GetSlotAsync(branchId, date);
            return Ok(new ResponseModel<List<AppointmentSlotResponseDto>>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                slots,
                "Get appointment slots successfully!"
            ));
        }

        [Authorize]
        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetById([FromRoute] string appointmentId)
        {
            var appointment = await _appointmentService.GetByIdAsync(appointmentId);
            return Ok(new ResponseModel<AppointmentResponseDto>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                appointment,
                "Get appointment successfully!"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentRequestDto requestDto)
        {
            await _appointmentService.CreateAsync(requestDto);
            return StatusCode(StatusCodes.Status201Created,
                new ResponseModel<string>(
                    StatusCodes.Status201Created,
                    ApiCodes.CREATED,
                    null,
                    "Created appointment successfully!"
                ));
        }

        [Authorize]
        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> Update([FromRoute] string appointmentId, [FromBody] AppointmentRequestDto requestDto)
        {
            await _appointmentService.UpdateAsync(appointmentId, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Updated appointment successfully!"
            ));
        }

        [Authorize]
        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> Delete([FromRoute] string appointmentId)
        {
            await _appointmentService.DeleteAsync(appointmentId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Deleted appointment successfully!"
            ));
        }

        [Authorize]
        [HttpPut("{id}/check-in")]
        public async Task<IActionResult> CheckIn(string id)
        {
            await _appointmentService.CheckInAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Checked-in appointment successfully!"
            ));
        }

        [Authorize]
        [HttpPut("{id}/check-out")]
        public async Task<IActionResult> CheckOut(string id)
        {
            await _appointmentService.CheckOutAsync(id);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Checked-out appointment successfully!"
            ));
        }

        [Authorize]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id, [FromBody] string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return BadRequest(new ResponseModel<string>(
                    StatusCodes.Status400BadRequest,
                    ApiCodes.BAD_REQUEST,
                    null,
                    "Cancel reason is required."
                ));
            }

            await _appointmentService.CancelAppointment(id, request);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ApiCodes.SUCCESS,
                null,
                "Cancelled appointment successfully!"
            ));
        }
    }
}
