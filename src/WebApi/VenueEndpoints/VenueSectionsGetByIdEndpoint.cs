using AutoMapper;
using System.Collections.Generic;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.VenueEndpoints;

public class VenueSectionsGetByIdEndpoint : IEndpoint<IResult, GetByIdVenueSectionsRequest, IRepository<Section>>
{
    private readonly IMapper _mapper;

    public VenueSectionsGetByIdEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.API_PREFIX + "/venues/{venueId}/sections",
            async (int venueId, IRepository<Section> sectionRepository) =>
            {
                return await HandleAsync(new GetByIdVenueSectionsRequest(venueId), sectionRepository);
            })
           .Produces<GetByIdVenueSectionsResponse>()
           .WithTags("VenueEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByIdVenueSectionsRequest request, IRepository<Section> sectionRepository)
    {
        var response = new GetByIdVenueSectionsResponse(request.CorrelationId());

        var sections = await sectionRepository.ListAsync();

        var sectionsByVenue = sections.FindAll(s => s.VenueId == request.VenueId);
        
        if (sections is null)
            return Results.NotFound();

        response.VenueSections.AddRange(sectionsByVenue.Select(_mapper.Map<SectionDto>));

        return Results.Ok(response);
    }
}
