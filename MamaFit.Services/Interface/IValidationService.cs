namespace MamaFit.Services.Interface;

public interface IValidationService
{
    Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default);
}