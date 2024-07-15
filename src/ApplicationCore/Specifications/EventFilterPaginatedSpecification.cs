using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.ApplicationCore.Specifications;

public class EventFilterPaginatedSpecification : Specification<Event>
{
    public EventFilterPaginatedSpecification(int skip, int take, int? venueId) : base()
    {
        if (take == 0)
            take = int.MaxValue;
        
        Query
            .Where(i => (!venueId.HasValue || i.VenueId == venueId))
            .Skip(skip).Take(take);
    }
}
