using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.ApplicationCore.Specifications;

public class SeatFilterBySectorIdSpec : Specification<Seat>
{
    public SeatFilterBySectorIdSpec(int? sectionId)
    {
        Query.Where(s => (sectionId.HasValue && s.SectionId == sectionId));
    }
}
