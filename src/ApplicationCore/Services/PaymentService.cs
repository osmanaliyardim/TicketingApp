using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Exceptions;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Services;

public class PaymentService : IPaymentService
{
    private readonly IRepository<Ticket> _ticketRepository;
    private readonly IRepository<Seat> _seatRepository;

    public PaymentService(
        IRepository<Ticket> ticketRepository, 
        IRepository<Seat> seatRepository)
    {
        _ticketRepository = ticketRepository;
        _seatRepository = seatRepository;
    }

    public async Task UpdateSeatStatusAsync(int ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);

        if (ticket == null)
            throw new TicketNotFoundException(ticketId);

        //var seats = ticket.Seats.ToList();
        var seat = ticket.SeatId;

        //var filterSpec = new SeatFilterSpecification(seatIds);
        //var seatToBook = await _seatRepository.ListAsync(filterSpec);
        var seatToBook = await _seatRepository.GetByIdAsync(seat);

        if (seatToBook.IsAvailable == false)
            throw new SeatAlreadyBookedException(seatToBook.Id);

        //seatsToBook.ForEach(s => s.IsAvailable = false);
        seatToBook.IsAvailable = false;

        // Make the payment here - If not successfull, then return error and set seat availability true again
        var paymentResult = await CompletePaymentAsync(ticket.BuyerId);
        if (paymentResult == false)
        {
            seatToBook.IsAvailable = true;

            throw new PaymentFailedException(ticketId);
        }
        //seatsToBook.ForEach(s => s.IsAvailable = true);
        else await _seatRepository.UpdateAsync(seatToBook);
        //await _seatRepository.UpdateRangeAsync(seatsToBook);
    }

    // Fake payment
    public async Task<bool> CompletePaymentAsync(int buyerId)
    {
        Random randomNumberGenerator = new();
        var number = 0;

        // 50% Success - 50% Fail Payments
        await Task.Run(() => { number = randomNumberGenerator.Next(1, 10); });

        return number > 5 ? true : false;
    }
}
