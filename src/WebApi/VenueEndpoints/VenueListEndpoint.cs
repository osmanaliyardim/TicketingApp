using AutoMapper;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;
using static System.Collections.Specialized.BitVector32;

namespace TicketingApp.WebApi.VenueEndpoints;

public class VenueListEndpoint : IEndpoint<IResult, IRepository<Venue>>
{
    private readonly IMapper _mapper;

    public VenueListEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.API_PREFIX}/venues",
            async (IRepository<Venue> venueRepository) =>
            {
                return await HandleAsync(venueRepository);
            })
           .Produces<ListVenuesResponse>()
           .WithTags("VenueEndpoints");
    }

    public async Task<IResult> HandleAsync(IRepository<Venue> venueRepository)
    {
        var response = new ListVenuesResponse();

        var items = await venueRepository.ListAsync();

        if (items is null)
            return Results.NotFound();

        response.Venues.AddRange(items.Select(_mapper.Map<VenueDto>));

        return Results.Ok(response);
    }
}
