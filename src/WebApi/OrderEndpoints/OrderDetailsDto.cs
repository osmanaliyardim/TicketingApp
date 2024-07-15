namespace TicketingApp.WebApi.OrderEndpoints;

public class OrderDetailsDto : OrderDto
{
    public List<OrderItemDto> OrderItems { get; set; } = new();
}
