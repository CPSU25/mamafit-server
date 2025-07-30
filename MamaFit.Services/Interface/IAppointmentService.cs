using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IAppointmentService
{
    Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize, DateTime? StartDate, DateTime? EndDate, AppointmentOrderBy? sortBy);
    Task<AppointmentResponseDto> GetByIdAsync(string id);
    Task<PaginatedList<AppointmentResponseDto>> GetByUserId(int index, int pageSize, string? search, AppointmentOrderBy? sortBy);
    Task<List<AppointmentSlotResponseDto>> GetSlotAsync(string branchId, DateOnly date);
    Task<string> CreateAsync(AppointmentRequestDto requestDto);
    Task UpdateAsync(string id, AppointmentRequestDto requestDto);
    Task DeleteAsync(string id);
    Task CheckInAsync(string id);
    Task CheckOutAsync(string id);
    Task CancelAppointment (string id, string reason);
}