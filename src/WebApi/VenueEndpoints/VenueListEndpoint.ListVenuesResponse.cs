namespace TicketingApp.WebApi.VenueEndpoints;

public class ListVenuesResponse : BaseResponse
{
    public ListVenuesResponse(Guid correlationId) : base(correlationId)
    {

    }

    public ListVenuesResponse()
    {

    }

    public List<VenueDto> Venues { get; set; } = new List<VenueDto>();
}
