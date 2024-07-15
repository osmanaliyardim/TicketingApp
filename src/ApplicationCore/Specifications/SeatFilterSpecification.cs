using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.ApplicationCore.Specifications;

public class SeatFilterSpecification : Specification<Seat>
{
    public SeatFilterSpecification(List<int>? seatIdList)
    {
        Query.Where(s => (seatIdList.Any() && seatIdList.Contains(s.Id)));
    }

    public SeatFilterSpecification(int? seatId)
    {
        Query.Where(s => (seatId.HasValue && s.Id == seatId));
    }
}
