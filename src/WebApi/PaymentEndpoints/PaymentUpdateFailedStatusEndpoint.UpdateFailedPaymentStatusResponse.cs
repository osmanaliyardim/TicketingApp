namespace TicketingApp.WebApi.PaymentEndpoints;

public class UpdateFailedPaymentStatusResponse : BaseResponse
{
    public UpdateFailedPaymentStatusResponse(Guid correlationId) : base(correlationId)
    {
    }

    public UpdateFailedPaymentStatusResponse()
    {
    }

    public string BookingResult => "Your book has been cancelled and seats are available now!";
}
