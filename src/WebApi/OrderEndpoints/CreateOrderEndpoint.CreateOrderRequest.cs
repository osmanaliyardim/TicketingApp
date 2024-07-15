using TicketingApp.WebApi.CartEndpoints;

namespace TicketingApp.WebApi.OrderEndpoints;

public class CreateOrderRequest : BaseRequest
{
    public CartDto Cart { get; set; }
}
