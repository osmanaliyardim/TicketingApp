namespace TicketingApp.ApplicationCore.Pipelines.Caching;

public interface ICacheRemoveRequest
{
    public string? CacheKey { get; }

    public bool BypassCache { get; }

    public string? CacheGroupKey { get; }
}
