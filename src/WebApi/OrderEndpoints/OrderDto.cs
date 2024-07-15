using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.WebApi.OrderEndpoints;

public class OrderDto
{
    public int OrderNumber { get; set; }

    public string BuyerId { get; set; }

    public DateTimeOffset OrderDate { get; set; }

    public Address ShipToAddress { get; set; }

    public decimal TotalPrice {  get; set; }

    private readonly List<OrderItem> OrderItems = new List<OrderItem>();

    private const string DEFAULT_STATUS = "Pending";

    public string Status => DEFAULT_STATUS;
}
