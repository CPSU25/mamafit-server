using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<List<Ticket>> GetAllTicketAsync()
        {
            var ticketList = await _dbSet
                .Include(x => x.OrderItem).ThenInclude(x => x.Order)
                .Include(x => x.OrderItem).ThenInclude(x => x.Preset).ThenInclude(x => x.Style)
                .Where(x => !x.IsDeleted).ToListAsync();

            return ticketList;
        }

        public async Task<List<Ticket>> GetAllTicketByUserIdAsync(string userId)
        {
            var ticketList = await _dbSet
                .Include(x => x.OrderItem).ThenInclude(x => x.Order)
                .Include(x => x.OrderItem).ThenInclude(x => x.Preset).ThenInclude(x => x.Style)
                .Where(x => !x.IsDeleted && x.OrderItem.Order.UserId == userId).ToListAsync();

            return ticketList;
        }

        public async Task<Ticket> GetTicketByIdAsync(string ticketId)
        {
            var ticket = await _dbSet
                .Include(x => x.OrderItem).ThenInclude(x => x.Order).ThenInclude(x => x.User)
                .Include(x => x.OrderItem).ThenInclude(x => x.Preset).ThenInclude(x => x.Style)
                .Where(x => !x.IsDeleted).FirstOrDefaultAsync();

            return ticket;
        }
    }
}
