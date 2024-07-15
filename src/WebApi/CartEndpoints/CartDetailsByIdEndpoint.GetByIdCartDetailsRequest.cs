namespace TicketingApp.WebApi.CartEndpoints;

public class GetByIdCartDetailsRequest : BaseRequest
{
    public int CartId { get; init; }

    public GetByIdCartDetailsRequest(int cartId)
    {
        CartId = cartId;
    }
}
