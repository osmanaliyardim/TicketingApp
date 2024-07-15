using TicketingApp.WebApi.Constants;
using AutoMapper;
using TicketingApp.ApplicationCore.Exceptions;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Services;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.WebApi.CartEndpoints;

public class CreateCartEndpoint : IEndpoint<IResult, CreateCartRequest>
{
    private readonly IBasketService _basketService;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CreateCartEndpoint(
        IBasketService basketService,
        ICartService cartService,
        IMapper mapper)
    {
        _basketService = basketService;
        _cartService = cartService;
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.API_PREFIX + "/orders/carts/{cartId}",
            //[Authorize(Roles = AuthorizationConstants.CUSTOMERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (CreateCartRequest request) =>
            {
                return await HandleAsync(request);
            })
            .Produces<CreateCartResponse>()
            .WithTags("CartEndpoints");
    }

    public async Task<IResult> HandleAsync(CreateCartRequest request)
    {
        var response = new CreateCartResponse(request.CorrelationId());

        var cart = new Basket(request.Cart.BuyerId);
        foreach (var item in request.Cart.Items)
        {
            cart = await _basketService.AddItemToBasket(
                                    request.Cart.BuyerId,
                                    item.EventId,
                                    item.UnitPrice,
                                    item.Quantity);
        }

        if (cart is null)
            throw new BasketNotFoundException(cart.Id);

        response.Cart = await _cartService.Map(cart);

        return Results.Created(ApiConstants.API_PREFIX + $"/orders/carts/{response.Cart.Id}", response);
    }
}
