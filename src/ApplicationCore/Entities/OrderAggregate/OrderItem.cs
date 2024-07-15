namespace TicketingApp.ApplicationCore.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public EventOrdered ItemOrdered { get; private set; }

    public decimal UnitPrice { get; private set; }

    public int Units { get; private set; }

    private OrderItem() {}

    public OrderItem(EventOrdered itemOrdered, decimal unitPrice, int units)
    {
        ItemOrdered = itemOrdered;
        UnitPrice = unitPrice;
        Units = units;
    }
}
