using System.Text.Json;
using FinancialPlanner.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace FinancialPlanner.Infrastructure.Caching;

public sealed class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrWhiteSpace(cachedValue))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedValue, SerializerOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan absoluteExpirationRelativeToNow,
        CancellationToken cancellationToken)
    {
        var serializedValue = JsonSerializer.Serialize(value, SerializerOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow
        };

        await distributedCache.SetStringAsync(key, serializedValue, options, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }
}
