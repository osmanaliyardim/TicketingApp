using Ardalis.Result;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.ApplicationCore.Interfaces;

public interface IBasketService
{
    Task TransferBasketAsync(string anonymousId, string userName);

    Task<Basket> AddItemToBasket(string userName, int eventId, decimal price, int quantity = 1);

    Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities);

    Task DeleteBasketAsync(int basketId);
}
