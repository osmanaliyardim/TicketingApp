using AutoMapper;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.EventEndpoints;

public class EventListPagedEndpoint : IEndpoint<IResult, ListPagedEventRequest, IRepository<Event>>
{
    private readonly IMapper _mapper;

    public EventListPagedEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.API_PREFIX}/events",
            async (int? pageSize, int? pageIndex, int? venueId, IRepository<Event> eventRepository) =>
            {
                return await HandleAsync(new ListPagedEventRequest(pageSize, pageIndex, venueId), eventRepository);
            })
            .Produces<ListPagedEventResponse>()
            .WithTags("EventEndpoints");
    }

    public async Task<IResult> HandleAsync(ListPagedEventRequest request, IRepository<Event> eventRepository)
    {
        await Task.Delay(1000);
        var response = new ListPagedEventResponse(request.CorrelationId());

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

        return Results.Ok(response);
    }
}
