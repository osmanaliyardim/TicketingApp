namespace TicketingApp.ApplicationCore.Entities;

public class Offer : BaseEntity
{
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Conditions { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; }
}
