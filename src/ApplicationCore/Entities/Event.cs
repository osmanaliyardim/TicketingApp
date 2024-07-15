using Ardalis.GuardClauses;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Entities;

public class Event : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public string Description { get; set; }

    public int VenueId { get; set; }

    public Venue Venue { get; set; }

    public Event(
        string description, string name,
        DateTime date, TimeSpan time,
        int venueId)
    {
        Description = description;
        Name = name;
        Date = date;
        Time = time;
        VenueId = venueId;
    }

    public void UpdateDetails(EventDetails details)
    {
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));

        Name = details.Name;
        Description = details.Description;
        Date = details.Date;
        Time = details.Time;
    }

    public void UpdateVenue(int venueId)
    {
        Guard.Against.Zero(venueId, nameof(venueId));
        VenueId = venueId;
    }

    public readonly record struct EventDetails
    {
        public string? Name { get; }
        public string? Description { get; }
        public DateTime Date { get; }
        public TimeSpan Time { get; }

        public EventDetails(
            string? name, string? description,
            DateTime date, TimeSpan time)
        {
            Name = name;
            Description = description;
            Date = date;
            Time = time;
        }
    }
}
