using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.PaymentEndpoints;

public class PaymentUpdateStatusEndpoint : IEndpoint<IResult, UpdatePaymentStatusRequest>
{
    private readonly IPaymentService _paymentService;
    private readonly IAppLogger<PaymentUpdateStatusEndpoint> _logger;

    public PaymentUpdateStatusEndpoint(
        IPaymentService paymentService,
        IAppLogger<PaymentUpdateStatusEndpoint> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.API_PREFIX + "/payments/{paymentId}/complete",
            //[Authorize(Roles = AuthorizationConstants.CUSTOMERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (int ticketId) =>
            {
                return await HandleAsync(new UpdatePaymentStatusRequest(ticketId));
            })
            .Produces<UpdatePaymentStatusResponse>()
            .WithTags("PaymentEndpoints");
    }

    public async Task<IResult> HandleAsync(UpdatePaymentStatusRequest request)
    {
        var response = new UpdatePaymentStatusResponse(request.CorrelationId());

        try
        {
            await _paymentService.UpdateSeatStatusAsync(request.TicketId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);

            throw new Exception(ex.Message);
        }

        return Results.Ok(response.BookingResult);
    }
}
