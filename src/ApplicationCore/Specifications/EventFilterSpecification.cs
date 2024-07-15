using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.ApplicationCore.Specifications;

public class EventFilterSpecification : Specification<Event>
{
    public EventFilterSpecification(int? venueId)
    {
        if (venueId == null)
            Query.AsNoTracking();

        else Query.Where(i => (venueId.HasValue && i.VenueId == venueId)).AsNoTracking();
    }
}
