using MamaFit.API.Constant;
using MamaFit.API.Middlewares;
using MamaFit.Configuration;
using NLog.Web;
using System.Text.Json.Serialization;

using MamaFit.BusinessObjects.DBContext;
using Microsoft.EntityFrameworkCore;
namespace MamaFit.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Add services to the container.
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("local"));
            });

            var logger = NLog.LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")).GetCurrentClassLogger();
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

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddApplicationServices(builder.Configuration);
                builder.Services.AddHttpClientServices();
                builder.Services.AddConfigSwagger();
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {

                    app.UseSwaggerUI();
                }
                else
                {
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

                app.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stop program because of exception");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
