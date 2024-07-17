using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketRemoveEmptyItems
{
    private readonly int _testEventId = 123;
    private readonly decimal _testUnitPrice = 1.23m;
    private readonly string _buyerId = "customer@ticketing.com";

    [Fact]
    public void RemovesEmptyBasketItems()
    {
        // Arrange
        var basket = new Basket(_buyerId);

        // Act
        basket.AddItem(_testEventId, _testUnitPrice, 0);
        basket.RemoveEmptyItems();

        // Assert
        //Assert.Equal(0, basket.Items.Count);
        Assert.Empty(basket.Items);
    }
}
