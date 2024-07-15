namespace TicketingApp.WebApi.PaymentEndpoints;

public class UpdatePaymentStatusResponse : BaseResponse
{
    public UpdatePaymentStatusResponse(Guid correlationId) : base(correlationId)
    {
    }

    public UpdatePaymentStatusResponse()
    {
    }

    public string BookingResult => "Your seat has been booked successfully!";
}
