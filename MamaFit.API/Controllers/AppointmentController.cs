using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "createdat_desc")
        {
            var appointments = await _appointmentService.GetAllAsync(index, pageSize, search, sortBy);
            return Ok(new ResponseModel<PaginatedList<AppointmentResponseDto>>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                appointments,
                null,
                "Get all appointments successfully!"
            ));
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetById([FromRoute] string appointmentId)
        {
            var appointment = await _appointmentService.GetByIdAsync(appointmentId);
            return Ok(new ResponseModel<AppointmentResponseDto>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                appointment,
                null,
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
                    ResponseCodeConstants.CREATED,
                    null,
                    null,
                    "Created appointment successfully!"
                ));
        }

        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> Update([FromRoute] string appointmentId, [FromBody] AppointmentRequestDto requestDto)
        {
            await _appointmentService.UpdateAsync(appointmentId, requestDto);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Updated appointment successfully!"
            ));
        }

        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> Delete([FromRoute] string appointmentId)
        {
            await _appointmentService.DeleteAsync(appointmentId);
            return Ok(new ResponseModel<string>(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                null,
                null,
                "Deleted appointment successfully!"
            ));
        }
    }
}
