using TicketingApp.ApplicationCore.Entities.BuyerAggregate;

namespace TicketingApp.ApplicationCore.Entities;

public class Ticket : BaseEntity
{
    public DateTime PurchaseDate { get; set; }

    public bool IsPrinted { get; set; }

    public int EventId { get; set; }

    public int BuyerId { get; set; }

    public int PricesId { get; set; }

    public int VenueId { get; set; }

    public Event Event { get; set; }

    public Buyer Buyer { get; set; }

    public Prices Prices { get; set; }

    public Venue Venue { get; set; }

    public Seat Seat { get; set; }

    public int SeatId { get; set; }
}
