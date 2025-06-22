using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.Redis;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<int> GetVersionAsync(string resource)
    {
        string key = $"{resource}:version";
        int? version = await GetAsync<int?>(key);
        return version ?? 1;
    }
    
    public async Task IncreaseVersionAsync(string resource, TimeSpan? expiry = null)
    {
        string key = $"{resource}:version";
        int version = await GetAsync<int?>(key) ?? 1;
        version++;
        await SetAsync(key, version, expiry ?? TimeSpan.FromDays(7));
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(data)) 
            return default;
        if (typeof(T) == typeof(int) && int.TryParse(data, out var intValue))
            return (T?)(object)intValue;

        if (typeof(T) == typeof(long) && long.TryParse(data, out var longValue))
            return (T?)(object)longValue;

        if (typeof(T) == typeof(bool) && bool.TryParse(data, out var boolValue))
            return (T?)(object)boolValue;
        return JsonConvert.DeserializeObject<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
        };
        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}