using MamaFit.API.Constant;
using MamaFit.API.Middlewares;
using MamaFit.API.DependencyInjection;
using NLog.Web;
using System.Text.Json.Serialization;
using FluentValidation;
using MamaFit.Repositories.Helper;
using MamaFit.Services.Validator;
using NLog;

namespace MamaFit.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")).GetCurrentClassLogger();
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
                
                builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
                builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
                builder.Services.AddValidatorsFromAssemblyContaining<ValidatorAssemblyReference>();
                builder.Services.AddDatabase(builder.Configuration);
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddApplicationServices(builder.Configuration);
                builder.Services.AddHttpClientServices();
                builder.Services.AddConfigSwagger();
                builder.Services.AddJwtAuthentication(builder.Configuration);
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
