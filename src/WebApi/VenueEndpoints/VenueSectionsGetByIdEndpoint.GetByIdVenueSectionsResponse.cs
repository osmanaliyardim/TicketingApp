namespace TicketingApp.WebApi.VenueEndpoints;

public class GetByIdVenueSectionsResponse : BaseResponse
{
    public GetByIdVenueSectionsResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetByIdVenueSectionsResponse()
    {
    }

    public List<SectionDto> VenueSections { get; set; } = new List<SectionDto>();
}
