using StackExchange.Redis;
using System.Text.Json;

namespace SmartShipping.Shared.Services;

public class RedisCacheService
{
    private readonly IDatabase _cache;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T data, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(data);
        await _cache.StringSetAsync(key, json, expiry);
    }
}
