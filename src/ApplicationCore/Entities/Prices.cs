namespace TicketingApp.ApplicationCore.Entities;

public class Prices : BaseEntity
{
    public Categories Category { get; set; }

    public decimal Price { get; set; }

    public int OfferId { get; set; }

    public Offer Offer { get; set; }
}

public enum Categories { Adult, Child, VIP }
