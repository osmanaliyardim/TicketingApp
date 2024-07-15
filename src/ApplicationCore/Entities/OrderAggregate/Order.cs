using Ardalis.GuardClauses;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Entities.OrderAggregate;

public class Order : BaseEntity, IAggregateRoot
{
    private Order() {}

    public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
    {
        Guard.Against.NullOrEmpty(buyerId, nameof(buyerId));

        BuyerId = buyerId;
        ShipToAddress = shipToAddress;
        _orderItems = items;
    }

    public string BuyerId { get; private set; }

    public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;

    public Address ShipToAddress { get; private set; }

    public decimal TotalPrice => Total();

    private readonly List<OrderItem> _orderItems = new List<OrderItem>();

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal Total()
    {
        var total = 0m;
        foreach (var item in _orderItems)
        {
            total += item.UnitPrice * item.Units;
        }
        return total;
    }
}
