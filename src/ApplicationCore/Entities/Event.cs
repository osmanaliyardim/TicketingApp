namespace TicketingApp.ApplicationCore.Entities;

public class Event : BaseEntity
{
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public string Description { get; set; }

    public int VenueId { get; set; }

    public Venue Venue { get; set; }
}
