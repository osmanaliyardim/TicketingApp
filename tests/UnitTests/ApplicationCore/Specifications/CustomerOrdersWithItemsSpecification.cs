using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.UnitTests.ApplicationCore.Specifications;

public class CustomerOrdersWithItemsSpecification
{
    private readonly string _buyerId = "customer@ticketing.com";
    private Address _shipToAddress = new Address("Ataturk St.", "Izmir", "35", "Turkiye", "35530");

    [Fact]
    public void ReturnsOrderWithOrderedItem()
    {
        // Arrange
        var spec = new TicketingApp.ApplicationCore.Specifications.CustomerOrdersWithItemsSpecification(_buyerId);
        
        // Act
        var result = spec.Evaluate(GetTestCollection()).FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderItems);
        //Assert.Equal(1, result.OrderItems.Count);
        Assert.Single(result.OrderItems);
        Assert.NotNull(result.OrderItems.FirstOrDefault()?.ItemOrdered);
    }

    [Fact]
    public void ReturnsAllOrderWithAllOrderedItem()
    {
        // Arrange
        var spec = new TicketingApp.ApplicationCore.Specifications.CustomerOrdersWithItemsSpecification(_buyerId);

        // Act
        var result = spec.Evaluate(GetTestCollection()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        //Assert.Equal(1, result[0].OrderItems.Count);
        Assert.Single(result[0].OrderItems);
        Assert.NotNull(result[0].OrderItems.FirstOrDefault()?.ItemOrdered);
        Assert.Equal(2, result[1].OrderItems.Count);
        Assert.NotNull(result[1].OrderItems.ToList()[0].ItemOrdered);
        Assert.NotNull(result[1].OrderItems.ToList()[1].ItemOrdered);
    }

    public List<Order> GetTestCollection()
    {
        var ordersList = new List<Order>();

        ordersList.Add(new Order(_buyerId, _shipToAddress,
            new List<OrderItem>
            {
                    new OrderItem(new EventOrdered(1, "Event1"), 10.50m, 1)
            }));
        ordersList.Add(new Order(_buyerId, _shipToAddress,
            new List<OrderItem>
            {
                    new OrderItem(new EventOrdered(2, "Event2"), 15.50m, 2),
                    new OrderItem(new EventOrdered(2, "Event3"), 20.50m, 1)
            }));

        return ordersList;
    }
}
