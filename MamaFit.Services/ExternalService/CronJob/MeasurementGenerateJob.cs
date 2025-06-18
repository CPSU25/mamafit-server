using MamaFit.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MamaFit.Services.ExternalService.CronJob;

public class MeasurementGenerationJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

    public MeasurementGenerationJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var measurementService = scope.ServiceProvider.GetRequiredService<IMeasurementService>();

      //      await measurementService.GenerateMissingMeasurementsAsync();

            await Task.Delay(_interval, stoppingToken);
        }
    }
}