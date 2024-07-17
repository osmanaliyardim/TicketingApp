using Ardalis.GuardClauses;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Extensions;

namespace TicketingApp.ApplicationCore.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<Event> _eventRepository;
    private readonly EventPublisher _eventPublisher;

    private const string SERVICE_BUS_CONNSTR = "<azure-service-bus-connection-string>";
    private const string SERVICE_BUS_QUEUE_NAME = "<azure-service-bus-queue-name>";

    public OrderService(IRepository<Basket> basketRepository,
        IRepository<Event> eventRepository,
        IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
        _basketRepository = basketRepository;
        _eventRepository = eventRepository;
        //_eventPublisher = new EventPublisher(SERVICE_BUS_CONNSTR, SERVICE_BUS_QUEUE_NAME);
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        Guard.Against.Null(basket, nameof(basket));
        Guard.Against.EmptyBasketOnCheckout(basket.Items);

        var eventItemsSpecification = new EventItemsSpecification(basket.Items.Select(item => item.EventId).ToArray());
        var eventItems = await _eventRepository.ListAsync(eventItemsSpecification);

        var items = basket.Items.Select(basketItem =>
        {
            var eventItem = eventItems.First(c => c.Id == basketItem.EventId);
            var itemOrdered = new EventOrdered(eventItem.Id, eventItem.Name);
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
            return orderItem;
        }).ToList();

        var order = new Order(basket.BuyerId, shippingAddress, items);

        await _orderRepository.AddAsync(order);

        // Send Order To Azure Service Bus
        //var orderedItems = new OrderedItems<Order>(Guid.NewGuid().ToString(), order);

        //await SendOrderEventToServiceBusAsync(orderedItems);
    }

    private async Task SendOrderEventToServiceBusAsync(OrderedItems<Order> order)
    {
        // Publish order event
        await _eventPublisher.PublishEventAsync(order);

        // Dispose of the EventPublisher
        await _eventPublisher.DisposeAsync();
    }
}
