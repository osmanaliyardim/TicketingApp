namespace TicketingApp.WebApi.EventEndpoints;

public class ListPagedEventResponse : BaseResponse
{
    public ListPagedEventResponse(Guid correlationId) : base(correlationId)
    {
    }

    public ListPagedEventResponse()
    {
    }

    public List<EventDto> Events { get; set; } = new List<EventDto>();

    public int PageCount { get; set; }
}
