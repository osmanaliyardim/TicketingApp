using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketTotalItems
{
    private readonly int _testEventId = 123;
    private readonly decimal _testUnitPrice = 1.23m;
    private readonly int _testQuantity = 2;
    private readonly string _buyerId = "customer@ticketing.com";

    [Fact]
    public void ReturnsTotalQuantityWithOneItem()
    {
        // Arrange
        var basket = new Basket(_buyerId);

        // Act
        basket.AddItem(_testEventId, _testUnitPrice, _testQuantity);
        var result = basket.TotalItems;

        // Assert
        Assert.Equal(_testQuantity, result);
    }

    [Fact]
    public void ReturnsTotalQuantityWithMultipleItems()
    {
        // Arrange
        var basket = new Basket(_buyerId);

        // Act
        basket.AddItem(_testEventId, _testUnitPrice, _testQuantity);
        basket.AddItem(_testEventId, _testUnitPrice, _testQuantity * 2);
        var result = basket.TotalItems;

        // Assert
        Assert.Equal(_testQuantity * 3, result);
    }
}
