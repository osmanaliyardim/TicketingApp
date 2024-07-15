using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.WebApi.EventEndpoints;

public record SeatDto
{
    public SeatTypes SeatType { get; set; }

    public string? Row { get; set; }

    public int Number { get; set; }

    public bool IsAvailable { get; set; }

    public int ManifestId { get; set; }

    public int SectionId { get; set; }
}
