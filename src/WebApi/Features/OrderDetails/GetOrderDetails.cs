using MediatR;
using TicketingApp.WebApi.OrderEndpoints;

namespace TicketingApp.WebApi.Features.OrderDetails;

public class GetOrderDetails : IRequest<OrderDetailsDto>
{
    public string UserName { get; set; }
    public int OrderId { get; set; }

    public GetOrderDetails(string userName, int orderId)
    {
        UserName = userName;
        OrderId = orderId;
    }
}
