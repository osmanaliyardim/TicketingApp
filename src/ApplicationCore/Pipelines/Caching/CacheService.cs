using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using TicketingApp.ApplicationCore.Constants;

namespace TicketingApp.ApplicationCore.Pipelines.Caching;

public class CacheService<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachableRequest
{
    private readonly CacheSettings _cacheSettings;
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache, IConfiguration configuration)
    {
        _cacheSettings = configuration.GetSection(CachingConstants.CACHE_SETTINGS).Get<CacheSettings>() ?? throw new InvalidOperationException();
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();

        TResponse response;

        byte[]? cachedResponse = (byte[]?)_cache.Get(request.CacheKey);

        if (cachedResponse != null)
            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
        else response = await GetResponseAndAddToCache(request, next, cancellationToken);

        return response;
    }

    private async Task<TResponse?> GetResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = await next();

        var slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
        var cacheOptions = new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration };

        byte[] serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

        _cache.Set(request.CacheKey, serializedData, cacheOptions);

        if (request.CacheGroupKey != null)
            await AddCacheKeyToGroup(request, slidingExpiration, cancellationToken);

        return response;
    }

    private async Task AddCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
    {
        byte[]? cacheGroupCache = (byte[]?)_cache.Get(request.CacheGroupKey!);
        HashSet<string> cacheKeysInGroup;

        if (cacheGroupCache != null)
        {
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;
            if (!cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
        }
        else
            cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });

        byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

        byte[]? cacheGroupCacheSlidingExpirationCache = (byte[]?)_cache.Get(
            $"{request.CacheGroupKey}SlidingExpiration"
        );

        int? cacheGroupCacheSlidingExpirationValue = null;
        if (cacheGroupCacheSlidingExpirationCache != null)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));

        if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);

        byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

        MemoryCacheEntryOptions cacheOptions =
            new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

        _cache.Set(request.CacheGroupKey!, newCacheGroupCache, cacheOptions);

        _cache.Set(
            $"{request.CacheGroupKey}SlidingExpiration",
            serializeCachedGroupSlidingExpirationData,
            cacheOptions
        );
    }
}