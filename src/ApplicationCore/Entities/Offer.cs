using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Entities;

public class Offer : BaseEntity, IAggregateRoot
{
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Conditions { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; }
}
