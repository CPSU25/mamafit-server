﻿using AutoMapper;
using Hangfire;
using MamaFit.BusinessObjects.DTO.AppointmentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.CronJob;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICacheService _cacheService;
        private readonly IValidationService _validationService;
        private readonly IConfigService _configService;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor,
            ICacheService cacheService, IValidationService validationService, IConfigService configService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _cacheService = cacheService;
            _validationService = validationService;
            _configService = configService;
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }

        public async Task<string> CreateAsync(AppointmentRequestDto requestDto)
        {
            var userName = GetCurrentUserName();

            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(requestDto.BranchId);
            _validationService.CheckNotFound(branch, $"Branch not found with id {requestDto.BranchId}");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(requestDto.UserId);
            _validationService.CheckNotFound(user, $"User not found with id {requestDto.UserId}");

            var dateOnly = DateOnly.FromDateTime(requestDto.BookingTime);
            var slotList = await GetSlotAsync(requestDto.BranchId, dateOnly);

            var bookingTime = TimeOnly.FromDateTime(requestDto.BookingTime).AddHours(7);

            // Kiểm tra nếu thời gian này đã bị đặt rồi (bị trùng)
            bool isDuplicated = slotList.Any(slot =>
                slot.IsBooked &&
                bookingTime == slot.Slot[0]);

            if (isDuplicated)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Booking time {bookingTime} is already booked by another user." +
                    $" Please choose another time slot.");


            // Kiểm tra nếu thời gian có nằm trong slot hợp lệ hay không
            bool isValid = slotList.Any(slot =>
                !slot.IsBooked &&
                bookingTime >= slot.Slot[0] &&
                bookingTime < slot.Slot[1]);

            if (!isValid)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Booking time {requestDto.BookingTime:t} is not within any available slot." +
                    $" Please choose a valid time range.");


            var config = await _configService.GetConfig();

            if (user.Appointments.Count(x =>
                    x.BookingTime.Date == requestDto.BookingTime.Date && x.Status != AppointmentStatus.CANCELED) >=
                config.Fields.MaxAppointmentPerDay)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"User {user.UserName} has reached the maximum number of appointments for the day" +
                    $". Please try again tomorrow.");

            if (user.Appointments.Count(x =>
                    x.Status == AppointmentStatus.UP_COMING && x.Status != AppointmentStatus.CANCELED) >=
                config.Fields.MaxAppointmentPerUser)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"User {user.UserName} has reached the maximum number of upcoming appointments" +
                    $". Please cancel an existing appointment before booking a new one.");

            var newAppointment = _mapper.Map<Appointment>(requestDto);
            newAppointment.User = user;
            newAppointment.Branch = branch;
            newAppointment.Status = AppointmentStatus.UP_COMING;
            newAppointment.CreatedAt = DateTime.UtcNow;
            newAppointment.CreatedBy = userName;
            newAppointment.UpdatedBy = userName;

            await _unitOfWork.AppointmentRepository.InsertAsync(newAppointment);
            await _unitOfWork.SaveChangesAsync();

            // Schedule nhắc trước 30'
            var scheduleUtc = newAppointment.BookingTime.AddMinutes(-30);

            if (scheduleUtc <= DateTime.UtcNow)
            {
                // Nếu lịch được đặt sát giờ (<=30'), gửi ngay lập tức bằng background
                BackgroundJob.Enqueue<IAppointmentReminderJob>(j => j.SendReminderAsync(newAppointment.Id));
            }
            else
            {
                var jobId = BackgroundJob.Schedule<IAppointmentReminderJob>(
                    j => j.SendReminderAsync(newAppointment.Id),
                    scheduleUtc);

                newAppointment.ReminderJobId = jobId;
                await _unitOfWork.AppointmentRepository.UpdateAsync(newAppointment);
                await _unitOfWork.SaveChangesAsync();
            }

            await _cacheService.RemoveByPrefixAsync($"appointment");

            return newAppointment.Id;
        }
        
        public async Task DeleteAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(oldAppointment, $"Appointment not found with id {id}");

            if (!string.IsNullOrEmpty(oldAppointment.ReminderJobId))
            {
                BackgroundJob.Delete(oldAppointment.ReminderJobId);
                oldAppointment.ReminderJobId = null;
                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.AppointmentRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            await _cacheService.RemoveByPrefixAsync($"appointment");
        }

        public async Task<PaginatedList<AppointmentResponseDto>> GetAllAsync(int index, int pageSize,
            DateTime? StartDate, DateTime? EndDate, AppointmentOrderBy? sortBy)
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                    $"User not found with id {userId}");
            var paginatedResponse =
                await _cacheService.GetAsync<PaginatedList<AppointmentResponseDto>>(
                    $"appointments_{index}_{pageSize}_{StartDate}_{EndDate}_{sortBy}");

            if (paginatedResponse == null)
            {
                var appointmentList =
                    await _unitOfWork.AppointmentRepository.GetAllAsync(userId, index, pageSize, StartDate, EndDate,
                        sortBy);

                // Map từng phần tử trong danh sách Items
                var responseList = appointmentList.Items.Select(item => _mapper.Map<AppointmentResponseDto>(item))
                    .ToList();

                // Tạo PaginatedList mới với các đối tượng đã map
                paginatedResponse = new PaginatedList<AppointmentResponseDto>(
                    responseList,
                    appointmentList.TotalCount,
                    appointmentList.PageNumber,
                    appointmentList.PageSize
                );

                await _cacheService.SetAsync($"appointment_{index}_{pageSize}_{StartDate}_{EndDate}_{sortBy}",
                    paginatedResponse);
                return paginatedResponse;
            }

            return paginatedResponse;
        }

        public async Task<AppointmentResponseDto> GetByIdAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdNotDeletedAsync(id);
            _validationService.CheckNotFound(oldAppointment, $"Appointment not found with id {id}");

            return _mapper.Map<AppointmentResponseDto>(oldAppointment);
        }

        public async Task UpdateAsync(string id, AppointmentRequestDto requestDto)
        {
            await _validationService.ValidateAndThrowAsync(requestDto);
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(appointment, $"Appointment not found with id {id}");

            if (!string.IsNullOrEmpty(appointment.ReminderJobId))
            {
                BackgroundJob.Delete(appointment.ReminderJobId);
                appointment.ReminderJobId = null;
                appointment.Reminder30SentAt = null; // reset nếu đổi giờ
            }

            _mapper.Map(requestDto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            var scheduleUtc = appointment.BookingTime.AddMinutes(-30);
            if (scheduleUtc <= DateTime.UtcNow)
            {
                BackgroundJob.Enqueue<IAppointmentReminderJob>(j => j.SendReminderAsync(appointment.Id));
            }
            else
            {
                var jobId = BackgroundJob.Schedule<IAppointmentReminderJob>(
                    j => j.SendReminderAsync(appointment.Id),
                    scheduleUtc);
                appointment.ReminderJobId = jobId;
                await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
                await _unitOfWork.SaveChangesAsync();
            }

            await _cacheService.RemoveByPrefixAsync($"appointment");
        }

        public async Task CheckInAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(oldAppointment, $"Appointment not found with id {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CHECKED_IN)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CHECKED_IN;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.RemoveByPrefixAsync($"appointment");
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.NOT_FOUND,
                    $"Appointment with id {id} already checked-in");
            }
        }

        public async Task CancelAppointment(string id, string reason)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(oldAppointment, $"Appointment not found with id {id}");

            if (oldAppointment.Status != BusinessObjects.Enum.AppointmentStatus.CANCELED)
            {
                oldAppointment.Status = BusinessObjects.Enum.AppointmentStatus.CANCELED;
                oldAppointment.CanceledAt = DateTime.UtcNow;
                oldAppointment.CanceledReason = reason;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.RemoveByPrefixAsync($"appointment");
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Appointment with id {id} already cancelled");
            }

            if (!string.IsNullOrEmpty(oldAppointment.ReminderJobId))
            {
                BackgroundJob.Delete(oldAppointment.ReminderJobId);
                oldAppointment.ReminderJobId = null;
                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task CheckOutAsync(string id)
        {
            var oldAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(oldAppointment, $"Appointment not found with id {id}");

            if (oldAppointment.Status != AppointmentStatus.CHECKED_OUT &&
                oldAppointment.Status != AppointmentStatus.CANCELED)
            {
                oldAppointment.Status = AppointmentStatus.CHECKED_OUT;
                oldAppointment.UpdatedAt = DateTime.UtcNow;
                oldAppointment.UpdatedBy = GetCurrentUserName();

                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.RemoveByPrefixAsync($"appointment");
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Appointment with id {id} already checked-out");
            }

            if (!string.IsNullOrEmpty(oldAppointment.ReminderJobId))
            {
                BackgroundJob.Delete(oldAppointment.ReminderJobId);
                oldAppointment.ReminderJobId = null;
                await _unitOfWork.AppointmentRepository.UpdateAsync(oldAppointment);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<PaginatedList<AppointmentResponseDto>> GetByUserId(int index, int pageSize, string? search,
            AppointmentOrderBy? sortBy)
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                    $"User not found with id {userId}");

            var appointmentList =
                await _unitOfWork.AppointmentRepository.GetByUserId(userId, index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = appointmentList.Items.Select(item => _mapper.Map<AppointmentResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<AppointmentResponseDto>(
                responseList,
                appointmentList.TotalCount,
                appointmentList.PageNumber,
                appointmentList.PageSize
            );

            return paginatedResponse;
        }

        private string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ?? "System";
        }

        public async Task<List<AppointmentSlotResponseDto>> GetSlotAsync(string branchId, DateOnly date)
        {
            var slotCacheKey = $"appointment_slots_{branchId}_{date}";
            var cachedSlots = await _cacheService.GetAsync<List<AppointmentSlotResponseDto>>(slotCacheKey);

            if (cachedSlots == null)
            {
                var branch = await _unitOfWork.BranchRepository.GetDetailById(branchId);
                _validationService.CheckNotFound(branch, $"Branch not found with id {branchId}");

                var config = await _configService.GetConfig();
                var slotInterval = TimeSpan.FromMinutes(config.Fields.AppointmentSlotInterval);

                var slots = await _unitOfWork.AppointmentRepository.GetSlot(branch, date, slotInterval);

                await _cacheService.SetAsync(slotCacheKey, slots); // Cache for 7 days
                return slots;
            }

            return cachedSlots;
        }
    }
}