using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketAddItem
{
    private readonly int _testEventId = 123;
    private readonly decimal _testUnitPrice = 1.23m;
    private readonly int _testQuantity = 2;
    private readonly string _buyerId = "customer@ticketing.com";

    [Fact]
    public void AddsEventToCartIfNotPresent()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act
        cart.AddItem(_testEventId, _testUnitPrice, _testQuantity);
        var firstItem = cart.Items.Single();

        // Assert
        Assert.Equal(_testEventId, firstItem.EventId);
        Assert.Equal(_testUnitPrice, firstItem.UnitPrice);
        Assert.Equal(_testQuantity, firstItem.Quantity);
    }

    [Fact]
    public void IncrementQuantityOfItemIfPresent()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act
        cart.AddItem(_testEventId, _testUnitPrice, _testQuantity);
        cart.AddItem(_testEventId, _testUnitPrice, _testQuantity);
        var firstItem = cart.Items.Single();

        // Assert
        Assert.Equal(_testQuantity * 2, firstItem.Quantity);
    }

    [Fact]
    public void KeepsOriginalUnitPriceIfMoreItemsAdded()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act
        cart.AddItem(_testEventId, _testUnitPrice, _testQuantity);
        cart.AddItem(_testEventId, _testUnitPrice * 2, _testQuantity);
        var firstItem = cart.Items.Single();

        // Assert
        Assert.Equal(_testUnitPrice, firstItem.UnitPrice);
    }

    [Fact]
    public void DefaultsToQuantityOfOne()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act
        cart.AddItem(_testEventId, _testUnitPrice);
        var firstItem = cart.Items.Single();

        // Assert
        Assert.Equal(1, firstItem.Quantity);
    }

    [Fact]
    public void CantAddItemWithNegativeQuantity()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddItem(_testEventId, _testUnitPrice, -1));
    }

    [Fact]
    public void CantModifyQuantityToNegativeNumber()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act
        cart.AddItem(_testEventId, _testUnitPrice);

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddItem(_testEventId, _testUnitPrice, -2));
    }
}
