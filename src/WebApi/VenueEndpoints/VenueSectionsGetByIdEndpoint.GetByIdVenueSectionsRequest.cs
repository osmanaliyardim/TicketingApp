namespace TicketingApp.WebApi.VenueEndpoints;

public class GetByIdVenueSectionsRequest : BaseRequest
{
    public int VenueId { get; init; }

    public GetByIdVenueSectionsRequest(int venueId)
    {
        VenueId = venueId;
    }
}
