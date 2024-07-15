using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.ApplicationCore.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(int basketId, Address shippingAddress);
}
