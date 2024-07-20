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

    public OrderService(IRepository<Basket> basketRepository,
        IRepository<Event> eventRepository,
        IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
        _basketRepository = basketRepository;
        _eventRepository = eventRepository;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        using (var transaction = await _orderRepository.BeginTransactionAsync())
        {
            try
            {
                // Retrieve the basket and lock it for update
                var basketSpec = new BasketWithItemsSpecification(basketId);
                var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

                Guard.Against.Null(basket, nameof(basket));
                Guard.Against.EmptyBasketOnCheckout(basket.Items);

                // Retrieve the event items and lock them for update
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

                // Commit the transaction
                await _orderRepository.CommitTransactionAsync();
            }
            catch (Exception)
            {
                // Rollback the transaction in case of an error
                await _orderRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
