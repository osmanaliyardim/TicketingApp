namespace TicketingApp.WebApi.PaymentEndpoints;

public class UpdatePaymentStatusRequest : BaseRequest
{
    public int TicketId { get; set; }

    public UpdatePaymentStatusRequest(int ticketId)
    {
        TicketId = ticketId;
    }
}
