namespace TicketingApp.WebApi.CartEndpoints;

public class CreateCartResponse : BaseResponse
{
    public CreateCartResponse(Guid correlationId) : base(correlationId)
    {
    }

    public CreateCartResponse()
    {
    }

    public CartDto Cart { get; set; }
}
