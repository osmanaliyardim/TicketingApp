namespace TicketingApp.WebApi.OrderEndpoints;

public class OrderItemDto
{
    public int EventId { get; set; }

    public string? EventName { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Discount => 0;

    public int Units { get; set; }
}
