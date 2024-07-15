using Ardalis.GuardClauses;

namespace TicketingApp.ApplicationCore.Entities.OrderAggregate;

public class EventOrdered // ValueObject
{
    public EventOrdered(int eventId, string eventName)
    {
        Guard.Against.OutOfRange(eventId, nameof(eventId), 1, int.MaxValue);
        Guard.Against.NullOrEmpty(eventName, nameof(eventName));

        EventId = eventId;
        EventName = eventName;
    }

    private EventOrdered() {}

    public int EventId { get; private set; }

    public string EventName { get; private set; }
}
