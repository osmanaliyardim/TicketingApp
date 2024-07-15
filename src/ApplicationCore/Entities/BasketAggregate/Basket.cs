using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.ApplicationCore.Entities.BasketAggregate;

public class Basket : BaseEntity, IAggregateRoot
{
    public string BuyerId { get; private set; }

    private readonly List<BasketItem> _items = new List<BasketItem>();

    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

    public int TotalItems => _items.Sum(i => i.Quantity);

    public Basket(string buyerId)
    {
        BuyerId = buyerId;
    }

    public void AddItem(int eventId, decimal unitPrice, int quantity = 1)
    {
        if (!Items.Any(i => i.EventId == eventId))
        {
            _items.Add(new BasketItem(eventId, quantity, unitPrice));
            return;
        }
        var existingItem = Items.First(i => i.EventId == eventId);
        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems()
    {
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void SetNewBuyerId(string buyerId)
    {
        BuyerId = buyerId;
    }
}
