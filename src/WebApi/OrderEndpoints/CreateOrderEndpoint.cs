using AutoMapper;
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

    public CreateOrderEndpoint(
        IBasketService basketService,
        IOrderService orderService,
        IMapper mapper,
        IAppLogger<CreateOrderEndpoint> logger)
    {
        _basketService = basketService;
        _orderService = orderService;
        _mapper = mapper;
        _logger = logger;
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
            //var response = new CreateOrderResponse(request.CorrelationId());

            var updateModel = request.Cart.Items.ToDictionary(b => b.Id.ToString(), b => b.Quantity);

            await _basketService.SetQuantities(request.Cart.Id, updateModel);
            await _orderService.CreateOrderAsync(request.Cart.Id, new Address("Ataturk St.", "Izmir", "35", "Turkiye", "35530"));
            await _basketService.DeleteBasketAsync(request.Cart.Id);
        }
        catch (EmptyBasketOnCheckoutException emptyBasketOnCheckoutException)
        {
            _logger.LogWarning(emptyBasketOnCheckoutException.Message);

            throw new EmptyBasketOnCheckoutException(emptyBasketOnCheckoutException.Message);
        }

        return Results.NoContent();
    }
}
