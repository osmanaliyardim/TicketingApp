using MediatR;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.OrderEndpoints;

namespace TicketingApp.WebApi.Features.MyOrders;

public class GetMyOrdersHandler : IRequestHandler<GetMyOrders, IEnumerable<OrderDto>>
{
    private readonly IReadRepository<Order> _orderRepository;

    public GetMyOrdersHandler(IReadRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetMyOrders request,
        CancellationToken cancellationToken)
    {
        var specification = new CustomerOrdersSpecification(request.UserName);
        var orders = await _orderRepository.ListAsync(specification, cancellationToken);

        return orders.Select(o => new OrderDto
        {
            BuyerId = o.BuyerId,
            OrderDate = o.OrderDate,
            ShipToAddress = o.ShipToAddress,
            TotalPrice = o.Total()
        });
    }
}
