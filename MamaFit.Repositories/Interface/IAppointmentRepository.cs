using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;


namespace MamaFit.Repositories.Interface
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<PaginatedList<Appointment>> GetAllAsync(string branchManagerId, int index, int pageSize, DateTime? StartDate, DateTime? EndDate, AppointmentOrderBy? sortBy);
        Task<PaginatedList<Appointment>> GetByUserId(string userId, int index, int pageSize, string? search, AppointmentOrderBy? sortBy);
        Task<List<AppointmentSlotResponseDto>> GetSlot(Branch branch, DateOnly date, TimeSpan slotInterval);
        Task<Appointment?> GetByIdNotDeletedAsync(string id);
    }
}
