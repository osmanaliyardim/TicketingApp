using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.WebApi.CartEndpoints;

namespace TicketingApp.WebApi.Services;

public interface ICartService
{
    Task<CartDto> GetOrCreateCartForUser(string userName);

    Task<int> CountTotalCartItems(string username);

    Task<CartDto> Map(Basket basket);
}
