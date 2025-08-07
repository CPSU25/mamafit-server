namespace MamaFit.Services.ExternalService.Redis;

public interface ICacheService
{
    Task<int> GetVersionAsync(string resource);
    Task IncreaseVersionAsync(string resource, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    // Redis SET native
    Task SetAddAsync(string key, string value, TimeSpan? expiry = null);
    Task SetRemoveAsync(string key, string value);
    Task<List<string>> SetMembersAsync(string key);
    Task<bool> KeyExistsAsync(string key);
    Task<long> SetLengthAsync(string key);
    Task KeyExpireAsync(string key, TimeSpan expiry);
    Task RemoveKeyAsync(string key);
    Task<List<string>> ScanKeysByPatternAsync(string pattern);
    Task RemoveByPrefixAsync(string prefix);
    #region Hash redis
    Task SetHashAsync<T>(string key, string field, T value, TimeSpan? expiry = null);
    Task<T?> GetHashAsync<T>(string key, string field);
    Task<Dictionary<string, T?>> GetAllHashAsync<T>(string key);
    Task<bool> DeleteHashFieldAsync(string key, string field);
    #endregion
}
