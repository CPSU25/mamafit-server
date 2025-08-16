using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<List<Ticket>> GetAllTicketAsync();
        Task<List<Ticket>> GetAllTicketByUserIdAsync(string userId);
        Task<Ticket> GetTicketByIdAsync(string ticketId);
    }
}
