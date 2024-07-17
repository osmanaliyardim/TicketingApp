using Moq;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Specifications;

namespace TicketingApp.UnitTests.ApplicationCore.Specifications;

public class BasketWithItems
{
    private readonly int _testBasketId = 123;
    private readonly string _buyerId = "customer@ticketing.com";

    [Fact]
    public void MatchesBasketWithGivenBasketId()
    {
        // Arrange
        var spec = new BasketWithItemsSpecification(_testBasketId);

        // Act
        var result = spec.Evaluate(GetTestBasketCollection()).FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testBasketId, result.Id);
    }

    [Fact]
    public void MatchesNoBasketsIfBasketIdNotPresent()
    {
        // Arrange
        int badBasketId = -1;

        // Act
        var spec = new BasketWithItemsSpecification(badBasketId);
        var result = spec.Evaluate(GetTestBasketCollection()).Any();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MatchesBasketWithGivenBuyerId()
    {
        // Arrange
        var spec = new BasketWithItemsSpecification(_buyerId);

        // Act
        var result = spec.Evaluate(GetTestBasketCollection()).FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_buyerId, result.BuyerId);
    }

    [Fact]
    public void MatchesNoBasketsIfBuyerIdNotPresent()
    {
        // Arrange
        string badBuyerId = "badBuyerId";

        // Act
        var spec = new BasketWithItemsSpecification(badBuyerId);
        var result = spec.Evaluate(GetTestBasketCollection()).Any();

        // Assert
        Assert.False(result);
    }

    public List<Basket> GetTestBasketCollection()
    {
        var basket1Mock = new Mock<Basket>(_buyerId);
        basket1Mock.Setup(item => item.Id).Returns(1);

        var basket2Mock = new Mock<Basket>(_buyerId);
        basket1Mock.Setup(item => item.Id).Returns(1);

        var basket3Mock = new Mock<Basket>(_buyerId);
        basket1Mock.Setup(item => item.Id).Returns(_testBasketId);

        return new List<Basket>()
            {
                basket1Mock.Object,
                basket2Mock.Object,
                basket3Mock.Object
            };
    }
}
