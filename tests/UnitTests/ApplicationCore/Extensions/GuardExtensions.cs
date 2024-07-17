using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Exceptions;
using Ardalis.GuardClauses;
using TicketingApp.ApplicationCore.Extensions;

namespace TicketingApp.UnitTests.ApplicationCore.Extensions;

public class GuardExtensions
{
    private readonly string _buyerId = "customer@ticketing.com";

    [Fact]
    public void CorrectlyThrowsExceptionDuringCheckoutWithEmptyCart()
    {
        // Arrange
        var cart = new Basket(_buyerId);

        // Act + Assert
        Assert.Throws<EmptyBasketOnCheckoutException>(() => Guard.Against.EmptyBasketOnCheckout(cart.Items));
    }
}
