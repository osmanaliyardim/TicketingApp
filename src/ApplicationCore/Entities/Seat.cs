using System.ComponentModel.DataAnnotations;

namespace TicketingApp.ApplicationCore.Entities;

public class Seat : BaseEntity
{
    [EnumDataType(typeof(SeatTypes))]
    public SeatTypes SeatType { get; set; }

    public string? Row { get; set; }

    public int Number { get; set; }

    public bool IsAvailable { get; set; }

    public int ManifestId { get; set; }

    public Manifest Manifest { get; set; }

    public int SectionId { get; set; }

    public Section Section { get; set; }
}

public enum SeatTypes { Row, GeneralAdmission }
