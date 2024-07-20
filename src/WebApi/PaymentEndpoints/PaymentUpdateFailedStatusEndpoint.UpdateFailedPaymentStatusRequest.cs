namespace TicketingApp.WebApi.PaymentEndpoints;

public class UpdateFailedPaymentStatusRequest : BaseRequest
{
    public int PaymentId { get; set; }

    public UpdateFailedPaymentStatusRequest(int paymentId)
    {
        PaymentId = paymentId;
    }
}
