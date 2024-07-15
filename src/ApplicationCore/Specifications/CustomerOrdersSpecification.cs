using Ardalis.Specification;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.ApplicationCore.Specifications;

public class CustomerOrdersSpecification : Specification<Order>
{
    public CustomerOrdersSpecification(string buyerId)
    {
        Query.Where(o => o.BuyerId == buyerId)
            .Include(o => o.OrderItems);
    }
}
