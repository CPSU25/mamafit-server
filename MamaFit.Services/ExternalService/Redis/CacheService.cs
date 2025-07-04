using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MamaFit.Services.ExternalService.Redis;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IDatabase _db;
    
    public CacheService(IDistributedCache cache, IConnectionMultiplexer redisDb)
    {
        _cache = cache;
        _db = redisDb.GetDatabase();
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
    
    //Redis SET native operations for signalR
    public async Task SetAddAsync(string key, string value, TimeSpan? expiry = null)
    {
        await _db.SetAddAsync(key, value);
        if (expiry.HasValue)
            await _db.KeyExpireAsync(key, expiry);
    }

    public async Task SetRemoveAsync(string key, string value)
    {
        await _db.SetRemoveAsync(key, value);
    }

    public async Task<List<string>> SetMembersAsync(string key)
    {
        var members = await _db.SetMembersAsync(key);
        return members.Select(x => x.ToString()).ToList();
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _db.KeyExistsAsync(key);
    }

    public async Task<long> SetLengthAsync(string key)
    {
        return await _db.SetLengthAsync(key);
    }

    public async Task KeyExpireAsync(string key, TimeSpan expiry)
    {
        await _db.KeyExpireAsync(key, expiry);
    }

    public async Task RemoveKeyAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
}