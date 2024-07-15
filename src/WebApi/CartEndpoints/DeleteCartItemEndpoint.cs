using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.CartEndpoints;

public class DeleteCartItemEndpoint : IEndpoint<IResult, DeleteCartItemRequest, IRepository<BasketItem>>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiConstants.API_PREFIX + "/orders/carts/{cartId}/events/{eventId}/seats/{seatId}",
            //[Authorize(Roles = AuthorizationConstants.CUSTOMERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (int basketItemId, IRepository<BasketItem> basketItemRepository) =>
            {
                return await HandleAsync(new DeleteCartItemRequest(basketItemId), basketItemRepository);
            })
            .Produces<DeleteCartItemResponse>()
            .WithTags("CartEndpoints");
    }

    public async Task<IResult> HandleAsync(DeleteCartItemRequest request, IRepository<BasketItem> basketItemRepository)
    {
        var response = new DeleteCartItemResponse(request.CorrelationId());

        var itemToDelete = await basketItemRepository.GetByIdAsync(request.CartItemId);
        if (itemToDelete is null)
            return Results.NotFound();

        await basketItemRepository.DeleteAsync(itemToDelete);

        return Results.Ok(response);
    }
}
