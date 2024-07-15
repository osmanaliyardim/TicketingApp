using MediatR;
using TicketingApp.WebApi.OrderEndpoints;

namespace TicketingApp.WebApi.Features.MyOrders;

public class GetMyOrders : IRequest<IEnumerable<OrderDto>>
{
    public string UserName { get; set; }

    public GetMyOrders(string userName)
    {
        UserName = userName;
    }
}
