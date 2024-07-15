namespace TicketingApp.ApplicationCore.Exceptions;

public class PaymentFailedException : Exception
{
    public PaymentFailedException(int ticketId) : base($"Payment failed for the ticket with id {ticketId}")
    {

    }
}
