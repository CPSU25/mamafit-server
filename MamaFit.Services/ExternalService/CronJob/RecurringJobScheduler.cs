using Hangfire;
using MamaFit.Services.Interface;

namespace MamaFit.Services.ExternalService.CronJob;

public class RecurringJobScheduler : IRecurringJobScheduler
{
    public void RegisterJob()
    {
        RecurringJob.AddOrUpdate<IMeasurementService>(
            "measurement-reminder-job",
            service => service.CheckAndSendRemindersAsync(),
            "0 9 * * *");
        
        RecurringJob.AddOrUpdate<IAppointmentReminderJob>(
            "appointment-reminder-sweeper",
            j => j.SweepAndSendAsync(),
            "* * * * *",
            timeZone: TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"));
    }
}