namespace TicketingApp.ApplicationCore.Pipelines.Caching;

public interface ICachableRequest
{
    public string CacheKey { get; }

    public bool BypassCache { get; }

    string? CacheGroupKey { get; }

    public TimeSpan? SlidingExpiration { get; }
}
