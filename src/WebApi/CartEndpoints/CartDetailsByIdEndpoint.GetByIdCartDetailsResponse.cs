namespace TicketingApp.WebApi.CartEndpoints;

public class GetByIdCartDetailsResponse : BaseResponse
{
    public GetByIdCartDetailsResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetByIdCartDetailsResponse()
    {
    }

    public CartDto Cart { get; set; }
}
