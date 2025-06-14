using FluentValidation;
using MamaFit.Services.Interface;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

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
                throw new FluentValidation.ValidationException(result.Errors);
        }
    }
}