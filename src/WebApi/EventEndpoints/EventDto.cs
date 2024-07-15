namespace TicketingApp.WebApi.EventEndpoints;

public record EventDto
{
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public string Description { get; set; }

    public int VenueId { get; set; }
}
