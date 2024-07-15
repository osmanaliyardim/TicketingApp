namespace TicketingApp.ApplicationCore.Interfaces;

public interface IPaymentService
{
    Task UpdateSeatStatusAsync(int ticketId);

    public Task<bool> CompletePaymentAsync(int buyerId);
}
