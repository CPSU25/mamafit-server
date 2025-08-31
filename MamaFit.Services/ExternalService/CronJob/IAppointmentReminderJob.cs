namespace MamaFit.Services.ExternalService.CronJob;

public interface IAppointmentReminderJob
{
    Task SendReminderAsync(string appointmentId);
    Task SweepAndSendAsync();
}