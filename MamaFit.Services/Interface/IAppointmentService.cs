using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.Repositories.Infrastructure;

public interface IAppointmentService
{
    Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy);
    Task<AppointmentResponseDto> GetByIdAsync(string id);
    Task CreateAsync(AppointmentRequestDto requestDto);
    Task UpdateAsync(string id, AppointmentRequestDto requestDto);
    Task DeleteAsync(string id);
}
