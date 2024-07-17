using Moq;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.UnitTests.Builders;

public class BasketBuilder
{
    private Basket _basket;
    public string BasketBuyerId => "customer@ticketing.com";

    public int BasketId => 1;

    public BasketBuilder()
    {
        _basket = WithNoItems();
    }

    public Basket Build()
    {
        return _basket;
    }

    public Basket WithNoItems()
    {
        var basketMock = new Mock<Basket>(BasketBuyerId);
        basketMock.Setup(b => b.Id).Returns(BasketId);

        _basket = basketMock.Object;

        return _basket;
    }

    public Basket WithOneBasketItem()
    {
        var basketMock = new Mock<Basket>(BasketBuyerId);
        _basket = basketMock.Object;
        _basket.AddItem(1, 10, 2);

        return _basket;
    }
}
