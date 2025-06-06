﻿using MamaFit.BusinessObjects.DTO.Appointment;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.Repositories.Infrastructure;

public interface IAppointmentService
{
    Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize, string? search, AppointmentOrderBy? sortBy);
    Task<AppointmentResponseDto> GetByIdAsync(string id);
    Task CreateAsync(AppointmentRequestDto requestDto);
    Task UpdateAsync(string id, AppointmentRequestDto requestDto);
    Task DeleteAsync(string id);
    Task CheckInAsync(string id);
    Task CheckOutAsync(string id);
    Task CancelAppointment (string id, string reason);
}
