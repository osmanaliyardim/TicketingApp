namespace TicketingApp.ApplicationCore.Exceptions;

public class TicketNotFoundException : Exception
{
    public TicketNotFoundException(int ticketId) : base($"No ticket found with id {ticketId}")
    {

    }
}
