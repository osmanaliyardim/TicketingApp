using Ardalis.GuardClauses;

namespace TicketingApp.ApplicationCore.Entities.BasketAggregate;

public class BasketItem : BaseEntity
{
    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public int EventId { get; private set; }

    public int BasketId { get; private set; }

    public BasketItem(int eventId, int quantity, decimal unitPrice)
    {
        EventId = eventId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }

    public void AddQuantity(int quantity)
    {
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity = quantity;
    }
}
