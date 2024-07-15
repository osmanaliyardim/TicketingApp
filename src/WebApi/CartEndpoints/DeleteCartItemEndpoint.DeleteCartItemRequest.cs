namespace TicketingApp.WebApi.CartEndpoints;

public class DeleteCartItemRequest : BaseRequest
{
    public int CartItemId { get; init; }

    public DeleteCartItemRequest(int cartItemId)
    {
        CartItemId = cartItemId;
    }
}
