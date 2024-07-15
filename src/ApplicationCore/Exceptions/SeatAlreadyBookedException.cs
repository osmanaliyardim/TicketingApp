namespace TicketingApp.ApplicationCore.Exceptions;

internal class SeatAlreadyBookedException : Exception
{
    public SeatAlreadyBookedException(int seatId) : base($"Seat already booked with id {seatId}")
    {

    }
}
