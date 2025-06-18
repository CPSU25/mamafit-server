namespace MamaFit.Services.Interface;

public interface IValidationService
{
    Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default);
    void CheckNotFound<T>(T? obj, string message) where T : class;
    void CheckConflict(bool condition, string message);
    void CheckBadRequest(bool condition, string message);
    void CheckForbidden(bool condition, string message);
    void CheckUnauthorized(bool condition, string message);
}