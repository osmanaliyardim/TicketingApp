using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.CartEndpoints;

namespace TicketingApp.WebApi.Services;

public class CartService : ICartService
{
    private readonly IRepository<Basket> _basketRepository;
    private readonly IBasketQueryService _basketQueryService;
    private readonly IRepository<Event> _eventRepository;

    public CartService(
        IRepository<Basket> basketRepository, 
        IBasketQueryService basketQueryService, 
        IRepository<Event> eventRepository)
    {
        _basketRepository = basketRepository;
        _basketQueryService = basketQueryService;
        _eventRepository = eventRepository;
    }

    public async Task<int> CountTotalCartItems(string username)
    {
        var counter = await _basketQueryService.CountTotalBasketItems(username);

        return counter;
    }

    public async Task<CartDto> GetOrCreateCartForUser(string userName)
    {
        var basketSpec = new BasketWithItemsSpecification(userName);
        var cart = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        if (cart == null)
        {
            return await CreateCartForUser(userName);
        }

        var cartDto = await Map(cart);

        return cartDto;
    }

    private async Task<CartDto> CreateCartForUser(string userId)
    {
        var cart = new Basket(userId);
        await _basketRepository.AddAsync(cart);

        return new CartDto()
        {
            BuyerId = cart.BuyerId,
            Id = cart.Id
        };
    }

    public async Task<CartDto> Map(Basket basket)
    {
        return new CartDto()
        {
            BuyerId = basket.BuyerId,
            Id = basket.Id,
            Items = await GetCartItems(basket.Items)
        };
    }

    private async Task<List<CartItemDto>> GetCartItems(IReadOnlyCollection<BasketItem> basketItems)
    {
        var eventItemsSpecification = new EventItemsSpecification(basketItems.Select(b => b.EventId).ToArray());
        var eventItems = await _eventRepository.ListAsync(eventItemsSpecification);

        var items = basketItems.Select(basketItem =>
        {
            var eventItem = eventItems.First(c => c.Id == basketItem.EventId);

            var cartItemDto = new CartItemDto
            {
                Id = basketItem.Id,
                UnitPrice = basketItem.UnitPrice,
                Quantity = basketItem.Quantity,
                EventId = basketItem.EventId,
                EventName = eventItem.Name
            };

            return cartItemDto;
        }).ToList();

        return items;
    }
}
