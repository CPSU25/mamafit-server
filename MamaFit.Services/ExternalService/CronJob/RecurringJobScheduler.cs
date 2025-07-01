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
    }
}