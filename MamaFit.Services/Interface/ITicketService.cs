using MamaFit.BusinessObjects.DTO.TicketDto;

namespace MamaFit.Services.Interface
{
    public interface ITicketService
    {
        Task<List<TicketResponseWithOrderDto>> GetAll();
        Task<List<TicketResponseWithOrderDto>> GetAllWithUserId();
        Task<TicketResponseWithOrderDto> GetById(string ticketId);
        Task CreateTicket(TicketRequestCreateDto request);
        Task UpdateTicket(TicketRequestUpdateDto request, string ticketId);
        Task DeleteTicket(string ticketId);
    }
}
