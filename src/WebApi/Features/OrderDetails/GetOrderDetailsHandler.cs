using MediatR;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.OrderEndpoints;

namespace TicketingApp.WebApi.Features.OrderDetails;

public class GetOrderDetailsHandler : IRequestHandler<GetOrderDetails, OrderDetailsDto?>
{
    private readonly IReadRepository<Order> _orderRepository;

    public GetOrderDetailsHandler(IReadRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDetailsDto?> Handle(GetOrderDetails request,
        CancellationToken cancellationToken)
    {
        var spec = new OrderWithItemsByIdSpec(request.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (order == null)
        {
            return null;
        }

        return new OrderDetailsDto
        {
            OrderDate = order.OrderDate,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                EventId = oi.ItemOrdered.EventId,
                EventName = oi.ItemOrdered.EventName,
                UnitPrice = oi.UnitPrice,
                Units = oi.Units
            }).ToList(),
            OrderNumber = order.Id,
            ShipToAddress = order.ShipToAddress,
            TotalPrice = order.Total()
        };
    }
}
