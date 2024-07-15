using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Entities;

public class Venue : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }

    public string Location { get; set; }

    public int Capacity { get; set; }
}
