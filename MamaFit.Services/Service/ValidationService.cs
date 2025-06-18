using FluentValidation;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        if (_serviceProvider.GetService(typeof(IValidator<T>)) is IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(instance, cancellationToken);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }
    
    public void CheckNotFound<T>(T? obj, string message) where T : class
    {
        if (obj == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, message);
    }

    public void CheckConflict(bool condition, string message)
    {
        if (condition)
            throw new ErrorException(StatusCodes.Status409Conflict, ApiCodes.CONFLICT, message);
    }

    public void CheckBadRequest(bool condition, string message)
    {
        if (condition)
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, message);
    }

    public void CheckForbidden(bool condition, string message)
    {
        if (condition)
            throw new ErrorException(StatusCodes.Status403Forbidden, ApiCodes.FORBIDDEN, message);
    }

    public void CheckUnauthorized(bool condition, string message)
    {
        if (condition)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ApiCodes.UNAUTHORIZED, message);
    }
}