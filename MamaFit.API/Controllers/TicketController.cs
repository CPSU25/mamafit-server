using Azure;
using MamaFit.BusinessObjects.DTO.TicketDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using MamaFit.Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTicket()
        {
            var response = await _ticketService.GetAll();

            return Ok(ResponseModel<List<TicketResponseWithOrderDto>>.OkResponseModel(response));
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetAllTicketByCurrentUser()
        {
            var response = await _ticketService.GetAllWithUserId();
            return Ok(ResponseModel<List<TicketResponseWithOrderDto>>.OkResponseModel(response));
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetById(string ticketId)
        {
            var response = await _ticketService.GetById(ticketId);
            return Ok(ResponseModel<TicketResponseWithOrderDto>.OkResponseModel(response));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketRequestCreateDto request)
        {
            await _ticketService.CreateTicket(request);

            return StatusCode(StatusCodes.Status201Created, ResponseModel<string>.CreatedResponseModel("Created successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTicket(TicketRequestUpdateDto request, [FromQuery] string ticketId)
        {
            await _ticketService.UpdateTicket(request, ticketId);

            return StatusCode(StatusCodes.Status200OK, ResponseModel<string>.OkResponseModel("Update successfully"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTicket([FromQuery] string ticketId)
        {
            await _ticketService.DeleteTicket(ticketId);

            return StatusCode(StatusCodes.Status200OK, ResponseModel<string>.OkResponseModel("Delete successfully"));
        }
    }
}
