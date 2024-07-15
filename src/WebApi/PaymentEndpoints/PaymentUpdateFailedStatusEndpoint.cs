using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.PaymentEndpoints;

public class PaymentUpdateFailedStatusEndpoint : IEndpoint<IResult, UpdateFailedPaymentStatusRequest>
{
    private readonly IPaymentService _paymentService;
    private readonly IAppLogger<PaymentUpdateFailedStatusEndpoint> _logger;

    public PaymentUpdateFailedStatusEndpoint(
        IPaymentService paymentService,
        IAppLogger<PaymentUpdateFailedStatusEndpoint> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.API_PREFIX + "/payments/{paymentId}/failed",
            //[Authorize(Roles = AuthorizationConstants.CUSTOMERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (int ticketId) =>
            {
                return await HandleAsync(new UpdateFailedPaymentStatusRequest(ticketId));
            })
            .Produces<UpdateFailedPaymentStatusResponse>()
            .WithTags("PaymentEndpoints");
    }

    public async Task<IResult> HandleAsync(UpdateFailedPaymentStatusRequest request)
    {
        var response = new UpdateFailedPaymentStatusResponse(request.CorrelationId());

        try
        {
            await _paymentService.UpdateSeatStatusAsync(request.PaymentId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);

            throw new Exception(ex.Message);
        }

        return Results.Ok(response.BookingResult);
    }
}
