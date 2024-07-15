namespace TicketingApp.WebApi.VenueEndpoints;

public record SectionDto
{
    public string Name { get; set; }

    public int VenueId { get; set; }
}
