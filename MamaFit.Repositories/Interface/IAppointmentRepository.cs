using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;


namespace MamaFit.Repositories.Interface
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<PaginatedList<Appointment>> GetAllAsync(int index, int pageSize, string? search, AppointmentOrderBy? sortBy);
        Task<PaginatedList<Appointment>> GetByUserId(string userId, int index, int pageSize, string? search, AppointmentOrderBy? sortBy);
    }
}
