using AutoMapper;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;

namespace TicketingApp.WebApi.CartEndpoints;

public class CartDetailsByIdEndpoint : IEndpoint<IResult, GetByIdCartDetailsRequest, IRepository<Basket>>
{
    private readonly IMapper _mapper;

    public CartDetailsByIdEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.API_PREFIX + "/orders/carts/{cartId}",
            //[Authorize(Roles = AuthorizationConstants.CUSTOMERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (int cartId, IRepository<Basket> cartRepository) =>
            {
                return await HandleAsync(new GetByIdCartDetailsRequest(cartId), cartRepository);
            })
           .Produces<GetByIdCartDetailsResponse>()
           .WithTags("CartEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByIdCartDetailsRequest request, IRepository<Basket> cartRepository)
    {
        var response = new GetByIdCartDetailsResponse(request.CorrelationId());

        var filterSpec = new BasketWithItemsSpecification(request.CartId);

        var cart = await cartRepository.FirstOrDefaultAsync(filterSpec);
        if (cart is null)
            return Results.NotFound();

        response.Cart = _mapper.Map<CartDto>(cart);

        return Results.Ok(response);
    }
}
