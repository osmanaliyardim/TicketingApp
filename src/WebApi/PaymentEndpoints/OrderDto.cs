using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.WebApi.PaymentEndpoints;

public class OrderDto
{
    public string BuyerId { get; set; }

    public DateTimeOffset OrderDate { get; set; }

    public Address ShipToAddress { get; private set; }

    public readonly List<OrderItem> OrderItems = new List<OrderItem>();

    public decimal TotalPrice { get; set; }
}
