namespace TicketingApp.WebApi.CartEndpoints;

public class DeleteCartItemResponse : BaseResponse
{
    public DeleteCartItemResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteCartItemResponse()
    {
    }

    public string Status { get; set; } = "Deleted";
}
