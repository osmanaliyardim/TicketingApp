using AutoMapper;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace TicketingApp.WebApi.EventEndpoints;

public class EventListPagedEndpoint : IEndpoint<IResult, ListPagedEventRequest, IRepository<Event>>
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private static readonly ConcurrentBag<string> EventCacheKeys = new();

    public EventListPagedEndpoint(IMapper mapper, IMemoryCache cache)
    {
        _mapper = mapper;
        _cache = cache;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.API_PREFIX}/events",
            async (HttpRequest httpRequest, HttpResponse httpResponse, int? pageSize, int? pageIndex, int? venueId, IRepository<Event> eventRepository) =>
            {
                return await HandleAsync(new ListPagedEventRequest(httpRequest, httpResponse, pageSize, pageIndex, venueId), eventRepository);
            })
            .Produces<ListPagedEventResponse>()
            .WithTags("EventEndpoints");
    }

    public async Task<IResult> HandleAsync(ListPagedEventRequest request, IRepository<Event> eventRepository)
    {
        var cacheKey = GenerateCacheKey(request);

        if (!_cache.TryGetValue(cacheKey, out ListPagedEventResponse response))
        {
            response = new ListPagedEventResponse(request.CorrelationId());

            var filterSpec = new EventFilterSpecification(request.VenueId);
            int totalItems = await eventRepository.CountAsync(filterSpec);

            var pagedSpec = new EventFilterPaginatedSpecification(
                skip: request.PageIndex * request.PageSize,
                take: request.PageSize,
                venueId: request.VenueId);

            var events = await eventRepository.ListAsync(pagedSpec);

            response.Events.AddRange(events.Select(_mapper.Map<EventDto>));

            if (request.PageSize > 0)
            {
                response.PageCount = int.Parse(Math.Ceiling((decimal)totalItems / request.PageSize).ToString());
            }
            else
            {
                response.PageCount = totalItems > 0 ? 1 : 0;
            }

            // Create cache key and bind it to main key, if the main key removed all child keys will be removed
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, response, cacheEntryOptions);

            // Track the cache key
            EventCacheKeys.Add(cacheKey);

            // Ensure the main key is set
            if (!_cache.TryGetValue(ApiConstants.MAIN_CACHE_KEY_FOR_EVENTS, out _))
            {
                _cache.Set(ApiConstants.MAIN_CACHE_KEY_FOR_EVENTS, EventCacheKeys, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.NeverRemove
                });
            }
        }

        // Generate E-TAG for client-side caching
        var eTag = GenerateETag(response);
        request.HttpResponse.Headers[HeaderNames.ETag] = eTag;
        request.HttpResponse.Headers[HeaderNames.CacheControl] = "public,max-age=60";

        if (request.HttpRequest.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var requestETag) && requestETag == eTag)
        {
            return Results.StatusCode(StatusCodes.Status304NotModified);
        }

        return Results.Ok(response);
    }

    private string GenerateCacheKey(ListPagedEventRequest request)
    {
        if (request.VenueId is null || request.VenueId == 0)
            return $"Events_{request.PageSize}_{request.PageIndex}";
        else return $"Events_{request.PageSize}_{request.PageIndex}_{request.VenueId}";
    }

    private string GenerateETag(ListPagedEventResponse response)
    {
        var responseString = System.Text.Json.JsonSerializer.Serialize(response);
        var responseBytes = Encoding.UTF8.GetBytes(responseString);
        var hash = System.Security.Cryptography.SHA256.HashData(responseBytes);

        return Convert.ToBase64String(hash);
    }
}
