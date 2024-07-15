namespace TicketingApp.WebApi.EventEndpoints;

public class GetByIdEventSeatsRequest : BaseRequest
{
    public int EventId { get; init; }

    public int SectionId { get; init; }

    public GetByIdEventSeatsRequest(int eventId, int sectionId)
    {
        EventId = eventId;
        SectionId = sectionId;
    }
}
