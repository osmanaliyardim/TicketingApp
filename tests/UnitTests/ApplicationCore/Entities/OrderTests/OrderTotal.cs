using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.UnitTests.Builders;

namespace TicketingApp.UnitTests.ApplicationCore.Entities.OrderTests;

public class OrderTotal
{
    private decimal _testUnitPrice = 42m;

    [Fact]
    public void IsZeroForNewOrder()
    {
        // Arrange + Act
        var order = new OrderBuilder().WithNoItems();

        // Assert
        Assert.Equal(0, order.Total());
    }

    [Fact]
    public void IsCorrectGiven1Item()
    {
        // Arrange + Act
        var builder = new OrderBuilder();
        var items = new List<OrderItem>
            {
                new OrderItem(builder.TestEventOrdered, _testUnitPrice, 1)
            };
        var order = new OrderBuilder().WithItems(items);

        // Assert
        Assert.Equal(_testUnitPrice, order.Total());
    }

    [Fact]
    public void IsCorrectGiven3Items()
    {
        // Arrange + Act
        var builder = new OrderBuilder();
        var order = builder.WithDefaultValues();

        // Assert
        Assert.Equal(builder.TestUnitPrice * builder.TestUnits, order.Total());
    }
}
