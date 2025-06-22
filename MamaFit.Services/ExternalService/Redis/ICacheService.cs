namespace MamaFit.Services.ExternalService.Redis;

public interface ICacheService
{
    Task<int> GetVersionAsync(string resource);
    Task IncreaseVersionAsync(string resource, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
}