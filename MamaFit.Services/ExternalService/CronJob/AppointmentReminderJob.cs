using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Services.Interface;

namespace MamaFit.Services.ExternalService.CronJob;

public class AppointmentReminderJob : IAppointmentReminderJob
{
    private readonly IUnitOfWork _uow;
    private readonly INotificationService _notification;
    private readonly IConfigService _config;
    
    public AppointmentReminderJob(
        IUnitOfWork uow,
        INotificationService notification,
        IConfigService config)
    {
        _uow = uow;
        _notification = notification;
        _config = config;
    }

    public async Task SendReminderAsync(string appointmentId)
    {
        var appt = await _uow.AppointmentRepository.GetByIdAsync(appointmentId);
        if (appt == null) return;

        // Không gửi nếu đã hủy / check-out / đã gửi
        if (appt.Status == AppointmentStatus.CANCELED ||
            appt.Status == AppointmentStatus.CHECKED_OUT ||
            appt.Reminder30SentAt != null) return;

        // Nếu quá giờ hoặc cách <0 phút => vẫn có thể gửi, tùy policy
        await SendAndMarkAsync(appt);
    }

    public async Task SweepAndSendAsync()
    {
        // Quét các lịch hẹn sẽ diễn ra trong [30, 31) phút tới, chưa gửi nhắc
        var from = DateTime.UtcNow.AddMinutes(30);
        var to   = DateTime.UtcNow.AddMinutes(31);

        var list = await _uow.AppointmentRepository.FindListAsync(a =>
            !a.IsDeleted &&
            a.Status == AppointmentStatus.UP_COMING &&
            a.Reminder30SentAt == null &&
            a.BookingTime >= from && a.BookingTime < to);

        foreach (var appt in list)
        {
            await SendAndMarkAsync(appt);
        }
    }

    private async Task SendAndMarkAsync(MamaFit.BusinessObjects.Entity.Appointment appt)
    {
        // Lấy giờ hiển thị theo VN
        var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows
        // Nếu chạy Linux, dùng "Asia/Ho_Chi_Minh"
        try { tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"); } catch {}

        var localTime = TimeZoneInfo.ConvertTimeFromUtc(appt.BookingTime, tz);

        await _notification.SendAndSaveNotificationAsync(new NotificationRequestDto
        {
            ReceiverId = appt.UserId!,
            NotificationTitle = "Nhắc lịch hẹn",
            Type = NotificationType.APPOINTMENT,
            NotificationContent = $"Bạn có lịch hẹn tại chi nhánh {(appt.Branch?.Name ?? "")} lúc {localTime:HH:mm dd/MM/yyyy}. Vui lòng đến đúng giờ nhé!",
            ActionUrl = $"/appointments/{appt.Id}",
            Metadata = new Dictionary<string, string>
            {
                { "appointmentId", appt.Id }
            }
        });

        appt.Reminder30SentAt = DateTime.UtcNow;
        await _uow.AppointmentRepository.UpdateAsync(appt);
        await _uow.SaveChangesAsync();
    }
}