namespace TicketingApp.WebApi.CartEndpoints;

public class CreateCartRequest : BaseRequest
{
    public CartDto Cart { get; set; }
}
