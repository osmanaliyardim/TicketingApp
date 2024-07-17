using AutoMapper;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;
using TicketingApp.ApplicationCore.Specifications;

namespace TicketingApp.WebApi.EventEndpoints;

public class EventSeatsGetByIdEndpoint : IEndpoint<IResult, GetByIdEventSeatsRequest, IRepository<Seat>>
{
    private readonly IMapper _mapper;

    public EventSeatsGetByIdEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.API_PREFIX + "/events/{eventId}/sections/{sectionId}/seats",
            async (int eventId, int sectionId, IRepository<Seat> seatRepository) =>
            {
                return await HandleAsync(new GetByIdEventSeatsRequest(eventId, sectionId), seatRepository);
            })
           .Produces<GetByIdEventSeatsResponse>()
           .WithTags("EventEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByIdEventSeatsRequest request, IRepository<Seat> seatRepository)
    {
        var response = new GetByIdEventSeatsResponse(request.CorrelationId());

        var filterSpec = new SeatFilterBySectorIdSpec(request.SectionId);

        var seats = await seatRepository.ListAsync(filterSpec);
        if (seats is null || !seats.Any())
            return Results.NotFound();

        response.Seats.AddRange(seats.Select(_mapper.Map<SeatDto>));

        return Results.Ok(response);
    }
}
