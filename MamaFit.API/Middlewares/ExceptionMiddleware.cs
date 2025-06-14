using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace MamaFit.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            try
            {
                await _next(context);
            }
            catch (CoreException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = ex.StatusCode;
                var result = JsonSerializer.Serialize(new { ex.Code, ex.Message, ex.AdditionalData });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            catch (ErrorException ex)
            {
                _logger.LogError(ex, ex.ErrorDetail.ErrorMessage.ToString());
                context.Response.StatusCode = ex.StatusCode;
                var result = JsonSerializer.Serialize(ex.ErrorDetail);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError(ex, "Validation failed.");

                var errors = ex.Errors.Select(err => new
                {
                    field = err.PropertyName,
                    message = err.ErrorMessage
                });

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    errorCode = ApiCodes.VALIDATION_FAILED,
                    errors
                });

                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var result = JsonSerializer.Serialize(new { error = $"An unexpected error occurred. Detail{ex.Message}" });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        }
    }
}
