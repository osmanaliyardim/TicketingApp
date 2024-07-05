namespace TicketingApp.ApplicationCore.Entities;

public class Manifest : BaseEntity
{
    public string SeatMap { get; set; }

    public int VenueId { get; set; }

    public Venue Venue { get; set; }
}
