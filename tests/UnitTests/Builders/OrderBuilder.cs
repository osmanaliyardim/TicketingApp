using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.UnitTests.Builders;

public class OrderBuilder
{
    private Order _order;

    public string TestBuyerId => "12345";

    public int TestEventId => 234;

    public string TestProductName => "Test Event Name";

    public decimal TestUnitPrice = 1.23m;

    public int TestUnits = 3;

    public EventOrdered TestEventOrdered { get; }

    public OrderBuilder()
    {
        TestEventOrdered = new EventOrdered(TestEventId, TestProductName);
        _order = WithDefaultValues();
    }

    public Order Build()
    {
        return _order;
    }

    public Order WithDefaultValues()
    {
        var orderItem = new OrderItem(TestEventOrdered, TestUnitPrice, TestUnits);
        var itemList = new List<OrderItem>() { orderItem };

        _order = new Order(TestBuyerId, new AddressBuilder().WithDefaultValues(), itemList);

        return _order;
    }

    public Order WithNoItems()
    {
        _order = new Order(TestBuyerId, new AddressBuilder().WithDefaultValues(), new List<OrderItem>());

        return _order;
    }

    public Order WithItems(List<OrderItem> items)
    {
        _order = new Order(TestBuyerId, new AddressBuilder().WithDefaultValues(), items);

        return _order;
    }
}
