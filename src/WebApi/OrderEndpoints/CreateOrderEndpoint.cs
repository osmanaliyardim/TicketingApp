using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.ApplicationCore.Exceptions;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.OrderEndpoints;

public class CreateOrderEndpoint : IEndpoint<IResult, CreateOrderRequest>
{
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IAppLogger<CreateOrderEndpoint> _logger;
    private readonly IMemoryCache _cache;

    public CreateOrderEndpoint(
        IBasketService basketService,
        IOrderService orderService,
        IMapper mapper,
        IAppLogger<CreateOrderEndpoint> logger,
        IMemoryCache cache)
    {
        _basketService = basketService;
        _orderService = orderService;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiConstants.API_PREFIX + "/orders/carts/{cartId}/book",
            //[Authorize(Roles = AuthorizationConstants.CUSTOMERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (CreateOrderRequest request) =>
            {
                return await HandleAsync(request);
            })
            .Produces<CreateOrderResponse>()
            .WithTags("OrderEndpoints");
    }

    public async Task<IResult> HandleAsync(CreateOrderRequest request)
    {
        try
        {
            var updateModel = request.Cart.Items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);

            await _basketService.SetQuantities(request.Cart.Id, updateModel);
            await _orderService.CreateOrderAsync(request.Cart.Id, new Address("Ataturk St.", "Izmir", "35", "Turkiye", "35530"));
            // ToDo: Uncomment it (for testing purpose)
            //await _basketService.DeleteBasketAsync(request.Cart.Id);

            // Invalidate all event cache entries
            InvalidateEventCache();
        }
        catch (EmptyBasketOnCheckoutException emptyBasketOnCheckoutException)
        {
            _logger.LogWarning(emptyBasketOnCheckoutException.Message);

            throw new EmptyBasketOnCheckoutException(emptyBasketOnCheckoutException.Message);
        }

        return Results.NoContent();
    }

    private void InvalidateEventCache()
    {
        if (_cache.TryGetValue(ApiConstants.MAIN_CACHE_KEY_FOR_EVENTS, out ConcurrentBag<string> eventCacheKeys))
        {
            foreach (var cacheKey in eventCacheKeys)
            {
                _cache.Remove(cacheKey);
            }

            // Remove the main key last
            _cache.Remove(ApiConstants.MAIN_CACHE_KEY_FOR_EVENTS);
        }
    }
}
