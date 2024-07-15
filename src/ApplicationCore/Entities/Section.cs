namespace TicketingApp.ApplicationCore.Entities;

public class Section : BaseEntity
{
    public string Name { get; set; }

    public int VenueId { get; set; }

    public Venue Venue { get; set; }
}
