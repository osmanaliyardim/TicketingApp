namespace TicketingApp.WebApi.OrderEndpoints;

public class CreateOrderResponse : BaseResponse
{
    public CreateOrderResponse(Guid correlationId) : base(correlationId)
    {

    }

    public CreateOrderResponse()
    {

    }
}
