using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace TicketingApp.ApplicationCore.Pipelines.Caching;

public class CacheRemovalService<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, ICacheRemoveRequest
{
    private readonly IMemoryCache _cache;

    public CacheRemovalService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();

        if (request.CacheGroupKey != null)
        {
            byte[]? cachedGroup = (byte[]?)_cache.Get(request.CacheGroupKey);

            if (cachedGroup != null)
            {
                HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cachedGroup));

                foreach (var key in keysInGroup)
                    _cache.Remove(key);

                _cache.Remove(request.CacheGroupKey);

                _cache.Remove(key: $"{request.CacheGroupKey}SlidingExpiration");
            }
        }

        var response = await next();

        if (request.CacheKey != null)
            _cache.Remove(request.CacheKey);

        return response;
    }
}
