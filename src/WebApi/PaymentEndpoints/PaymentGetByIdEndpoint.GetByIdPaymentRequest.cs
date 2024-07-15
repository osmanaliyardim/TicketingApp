namespace TicketingApp.WebApi.PaymentEndpoints;

public class GetByIdPaymentRequest : BaseRequest
{
    public string BuyerId { get; set; }

    public GetByIdPaymentRequest(string buyerId)
    {
        BuyerId = buyerId;
    }
}
