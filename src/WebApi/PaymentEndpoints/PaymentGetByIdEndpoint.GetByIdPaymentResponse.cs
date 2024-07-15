namespace TicketingApp.WebApi.PaymentEndpoints;

public class GetByIdPaymentResponse : BaseResponse
{
    public GetByIdPaymentResponse(Guid correlationId) : base(correlationId)
    {

    }

    public GetByIdPaymentResponse()
    {

    }

    public bool IsSuccess { get; set; }
}
