using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.ApplicationCore.Specifications;

public class EventItemsSpecification : Specification<Event>
{
    public EventItemsSpecification(params int[] ids)
    {
        Query.Where(c => ids.Contains(c.Id));
    }
}
