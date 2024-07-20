namespace TicketingApp.WebApi.EventEndpoints;

public class ListPagedEventRequest : BaseRequest
{
    public int PageSize { get; init; }

    public int PageIndex { get; init; }

    public int? VenueId { get; init; }

    public HttpRequest HttpRequest { get; init; }

    public HttpResponse HttpResponse { get; init; }

    public ListPagedEventRequest(HttpRequest httpRequest, HttpResponse httpResponse, int? pageSize, int? pageIndex, int? venueId)
    {
        PageSize = pageSize ?? 0;
        PageIndex = pageIndex ?? 0;
        VenueId = venueId;
        HttpRequest = httpRequest;
        HttpResponse = httpResponse;
    }
}
