using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
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

        public async Task<Appointment?> GetByIdNotDeletedAsync(string id)
        {
            return await _dbSet
                .Include(x => x.User)
                .ThenInclude(x => x!.Role)
                .Include(x => x.Branch)
                .ThenInclude(x => x.BranchManager)
                .ThenInclude(x => x!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
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
                .ThenInclude(x => x.BranchManager)
                .ThenInclude(x => x!.Role)
               .AsNoTracking()
               .Where(a => !a.IsDeleted && a.User != null && a.User.Id == userId);

            // Optional: apply search filter if needed

            query = sortBy switch
            {
                AppointmentOrderBy.UPCOMMING_AT_ASC => query
                    .OrderBy(u => u.BookingTime),

                AppointmentOrderBy.UPCOMMING_AT_DESC => query
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

        public async Task<List<AppointmentSlotResponseDto>> GetSlot(Branch branch, DateOnly date, TimeSpan slotInterval)
        {
            var dateTime = date.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();

            var bookedSlots = await _dbSet
                .AsNoTracking()
                .Where(a => !a.IsDeleted 
                && a.BranchId == branch.Id
                && a.BookingTime.Date == dateTime.Date
                && a.Status != AppointmentStatus.CANCELED)
                .Select(a => TimeOnly.FromDateTime(a.BookingTime))
                .ToListAsync();

            var slots = new List<AppointmentSlotResponseDto>();

            var currentTime = branch.OpeningHour.AddHours(-7);
            var workEnd = branch.ClosingHour.AddHours(-7);

            // Tạo các slot chia đều theo slotInterval
            while (currentTime.Add(slotInterval) <= workEnd)
            {
                var slotStart = currentTime;
                var slotEnd = currentTime.Add(slotInterval);

                // Kiểm tra xem slotStart đã được đặt chưa
                bool isBooked = bookedSlots.Any(b => b >= slotStart && b < slotEnd);

                slots.Add(new AppointmentSlotResponseDto
                {
                    Slot = new List<TimeOnly> { slotStart.AddHours(7), slotEnd.AddHours(7) },
                    IsBooked = isBooked
                });

                currentTime = slotEnd;
            }

            return slots;
        }
    }
}
