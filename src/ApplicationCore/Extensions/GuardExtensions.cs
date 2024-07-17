using Ardalis.GuardClauses;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Exceptions;

namespace TicketingApp.ApplicationCore.Extensions;

public static class BasketGuards
{
    public static void EmptyBasketOnCheckout(this IGuardClause guardClause, IReadOnlyCollection<BasketItem> basketItems)
    {
        if (!basketItems.Any())
            throw new EmptyBasketOnCheckoutException();
    }
}
