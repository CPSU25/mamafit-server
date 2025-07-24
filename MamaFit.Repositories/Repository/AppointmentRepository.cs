using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {

        public AppointmentRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<PaginatedList<Appointment>> GetAllAsync(int index, int pageSize, string? search, AppointmentOrderBy? sortBy)
        {
            var query = _dbSet.AsNoTracking()
                .Where(a => !a.IsDeleted);

            query = sortBy switch
            {
                AppointmentOrderBy.UPCOMMING_AT_ASC => query
                .Where(u => u.BookingTime > DateTime.UtcNow)
                .OrderBy(u => u.BookingTime),

                AppointmentOrderBy.UPCOMMING_AT_DESC => query
                .Where(u => u.BookingTime > DateTime.UtcNow)
                .OrderByDescending(u => u.BookingTime),

                AppointmentOrderBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),

                AppointmentOrderBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var listAppointment = pagedResult.Items
                .ToList();

            var responseAppointmentList = new PaginatedList<Appointment>
                (listAppointment, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);

            return responseAppointmentList;
        }

        public async Task<PaginatedList<Appointment>> GetByUserId(string userId, int index, int pageSize, string? search, AppointmentOrderBy? sortBy)
        {
            var query = _dbSet
                .Include(x => x.User)
                .ThenInclude(x => x.Role)
                .Include(x => x.Branch)
                .Include(x => x.Staff)
               .AsNoTracking()
               .Where(a => !a.IsDeleted && a.User != null && a.User.Id == userId);

            // Optional: apply search filter if needed

            query = sortBy switch
            {
                AppointmentOrderBy.UPCOMMING_AT_ASC => query
                    .Where(u => u.BookingTime > DateTime.UtcNow)
                    .OrderBy(u => u.BookingTime),

                AppointmentOrderBy.UPCOMMING_AT_DESC => query
                    .Where(u => u.BookingTime > DateTime.UtcNow)
                    .OrderByDescending(u => u.BookingTime),

                AppointmentOrderBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),

                AppointmentOrderBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),

                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            var pagedResult = await GetPaging(query, index, pageSize);

            var listAppointment = pagedResult.Items
                .ToList();

            return new PaginatedList<Appointment>(listAppointment, pagedResult.TotalCount, pagedResult.PageNumber, pageSize);
        }
    }
}
