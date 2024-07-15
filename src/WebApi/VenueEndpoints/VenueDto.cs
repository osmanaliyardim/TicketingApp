namespace TicketingApp.WebApi.VenueEndpoints;

public record VenueDto
{
    public string Name { get; set; }

    public string Location { get; set; }

    public int Capacity { get; set; }
}
