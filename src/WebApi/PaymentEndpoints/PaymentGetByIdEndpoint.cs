using AutoMapper;
using TicketingApp.ApplicationCore.Entities.BuyerAggregate;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.PaymentEndpoints;

public class PaymentGetByIdEndpoint : IEndpoint<IResult, GetByIdPaymentRequest, IRepository<Buyer>>
{
    private readonly IMapper _mapper;

    public PaymentGetByIdEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.API_PREFIX + "/payments/{paymentId}",
            async (string buyerId, IRepository<Buyer> buyerRepository) =>
            {
                return await HandleAsync(new GetByIdPaymentRequest(buyerId), buyerRepository);
            })
           .Produces<GetByIdPaymentResponse>()
           .WithTags("PaymentEndpoints");
    }

    public async Task<IResult> HandleAsync(GetByIdPaymentRequest request, IRepository<Buyer> buyerRepository)
    {
        var response = new GetByIdPaymentResponse(request.CorrelationId());

        var filterSpec = new CustomerOrdersWithItemsSpecification(request.BuyerId);
        var payment = await buyerRepository.GetByIdAsync(filterSpec);
        
        if (payment is null)
            return Results.NotFound();

        response.IsSuccess = true;

        return Results.Ok(response);
    }
}
