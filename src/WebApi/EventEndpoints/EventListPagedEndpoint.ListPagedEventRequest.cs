namespace TicketingApp.WebApi.EventEndpoints;

public class ListPagedEventRequest : BaseRequest
{
    public int PageSize { get; init; }

    public int PageIndex { get; init; }

    public int? VenueId { get; init; }

    public ListPagedEventRequest(int? pageSize, int? pageIndex, int? venueId)
    {
        PageSize = pageSize ?? 0;
        PageIndex = pageIndex ?? 0;
        VenueId = venueId;
    }
}
