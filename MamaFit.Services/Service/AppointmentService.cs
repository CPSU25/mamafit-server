using System.Globalization;
using AutoMapper;
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
        private readonly IEmailSenderSevice _emailSenderService;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor,
            ICacheService cacheService, IValidationService validationService, IConfigService configService,
            IEmailSenderSevice emailSenderService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _cacheService = cacheService;
            _validationService = validationService;
            _configService = configService;
            _emailSenderService = emailSenderService;
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
                var jobId = BackgroundJob.Enqueue<IAppointmentReminderJob>(j => j.SendReminderAsync(newAppointment.Id));

                newAppointment.ReminderJobId = jobId;

                await _unitOfWork.AppointmentRepository.UpdateAsync(newAppointment);
                await _unitOfWork.SaveChangesAsync();
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

            await SendAppointmentConfirmationEmailAsync(newAppointment);

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
                appointment.ReminderJobId = null; // Reset ReminderJobId
                appointment.Reminder30SentAt = null; // Reset Reminder30SentAt if time changes
            }

            _mapper.Map(requestDto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            var scheduleUtc = appointment.BookingTime.AddMinutes(-30);

            if (scheduleUtc <= DateTime.UtcNow)
            {
                // If the appointment is very soon (<= 30 minutes), send reminder immediately
                var jobId = BackgroundJob.Enqueue<IAppointmentReminderJob>(j => j.SendReminderAsync(appointment.Id));

                appointment.ReminderJobId = jobId;

                await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
                await _unitOfWork.SaveChangesAsync();
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

        public async Task SendAppointmentConfirmationEmailAsync(Appointment newAppointment)
        {
            var user = newAppointment.User;
            var email = user.UserEmail;
            if (string.IsNullOrWhiteSpace(email))
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "User email is not set, cannot send appointment confirmation.");

            var subject = "[MamaFit] Đặt lịch hẹn thành công";

            // Build the HTML content for the email
            var htmlContent = BuildAppointmentConfirmationHtml(newAppointment);

            // Use an email sender service to send the email (this is just an example, replace with your actual email service)
            await _emailSenderService.SendEmailAsync(email, subject, htmlContent);
        }

        private string BuildAppointmentConfirmationHtml(Appointment appointment)
        {
            var vn = new CultureInfo("vi-VN");

            // Convert UTC to Vietnam time (UTC +7)
            var appointmentTimeInVietnam = appointment.BookingTime.ToUniversalTime().AddHours(7);

            // Format the time in Vietnam local time
            var appointmentTime = appointmentTimeInVietnam.ToString("HH:mm dd/MM/yyyy");

            return $@"
    <!DOCTYPE html>
    <html lang=""vi"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Đặt lịch hẹn thành công</title>
        <style>
            body {{ font-family: Arial, Helvetica, sans-serif; background:#f7f7f7; margin:0; padding:0; }}
            .container {{ max-width: 600px; margin:40px auto; background:#fff; border-radius:8px; box-shadow:0 2px 10px rgba(0,0,0,0.05); padding:24px; }}
            .brand {{ font-size:22px; font-weight:bold; color:#2266cc; text-align:center; margin-bottom:6px; }}
            .sub {{ text-align:center; color:#666; margin-bottom:16px; }}
            .section-title {{ font-size:16px; font-weight:bold; margin:18px 0 8px; }}
            .table {{ width:100%; border-collapse:collapse; }}
            .table th, .table td {{ border-bottom:1px solid #eee; padding:8px 0; font-size:14px; }}
            .right {{ text-align:right; }}
            .footer {{ margin-top:24px; font-size:12px; color:#888; text-align:center; }}
            .badge {{ display:inline-block; padding:6px 10px; background:#e8f3ff; color:#2266cc; border-radius:999px; font-size:12px; }}
        </style>
    </head>
    <body>
    <div class=""container"">
        <div class=""brand"">MamaFit</div>
        <div class=""sub""><span class=""badge"">Đặt lịch hẹn thành công</span></div>

        <div class=""section-title"">Thông tin lịch hẹn</div>
        <table class=""table"">
            <tr><td>Trạng thái</td><td class=""right"">Chờ xác nhận</td></tr>
            <tr><td>Thời gian</td><td class=""right"">{appointmentTime}</td></tr>
            <tr><td>Chi nhánh</td><td class=""right"">{appointment.Branch?.Name}</td></tr>
        </table>

        <div class=""section-title"">Cảm ơn bạn đã đặt lịch với MamaFit!</div>
        <div class=""footer"">
            Nếu có sai sót, vui lòng phản hồi email này hoặc liên hệ MamaFit để được hỗ trợ.<br/>
            &copy; {DateTime.Now.Year} MamaFit. All rights reserved.
        </div>
    </div>
    </body>
    </html>";
        }
    }
}