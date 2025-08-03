using MamaFit.API.Constant;
using MamaFit.API.Middlewares;
using MamaFit.Services.Hubs;
using MamaFit.API.DependencyInjection;
using NLog.Web;
using System.Text.Json.Serialization;
using FluentValidation;
using Hangfire;
using MamaFit.Repositories.Helper;
using MamaFit.Services.ExternalService.AI;
using MamaFit.Services.ExternalService.CronJob;
using MamaFit.Services.ExternalService.Filter;
using MamaFit.Services.Validator;
using NLog;

namespace MamaFit.API
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")).GetCurrentClassLogger();
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: CorsConstant.PolicyName,
                        policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
                });

                builder.Services.AddControllers().AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

                builder.Services.AddRedisCache(builder.Configuration);
                builder.Services.AddHangfireWithProgres(builder.Configuration);
                builder.Services.Configure<SepaySettings>(builder.Configuration.GetSection("SepaySettings"));
                builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
                builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
                builder.Services.Configure<GhtkSettings>(builder.Configuration.GetSection("GhtkSettings"));
                builder.Services.AddGhtkClient(builder.Configuration);
                builder.Services.AddValidatorsFromAssemblyContaining<ValidatorAssemblyReference>();
                builder.Services.AddDatabase(builder.Configuration);
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddApplicationServices(builder.Configuration);
                builder.Services.AddAIServices(builder.Configuration);
                builder.Services.AddHttpClientServices();
                builder.Services.AddConfigSwagger();
                builder.Services.AddJwtAuthentication(builder.Configuration);
                var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var aiService = scope.ServiceProvider.GetService<IAIMeasurementCalculationService>();
                    if (aiService != null)
                    {
                        var isAvailable = await aiService.IsAvailable();
                        logger.Info($"AI Service Status at startup: {(isAvailable ? "Available" : "Not Available")}");
                    }
                }
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                else
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MamaFit API V1");
                        c.RoutePrefix = string.Empty;
                    });
                }

                app.UseMiddleware<ExceptionMiddleware>();
                
                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseCors(CorsConstant.PolicyName);

                app.UseSwagger();

                app.UseAuthentication();

                app.UseAuthorization();

                app.MapControllers();

                app.MapHub<ChatHub>("/chatHub");
                app.MapHub<NotificationHub>("/notificationHub");

                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
                });

                using (var scope = app.Services.CreateScope())
                {
                    var recurringJobScheduler = scope.ServiceProvider.GetRequiredService<IRecurringJobScheduler>();
                    recurringJobScheduler.RegisterJob();
                }
                
                app.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stop program because of exception");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
