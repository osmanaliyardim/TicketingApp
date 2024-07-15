using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.Infrastructure.Identity;
using Swashbuckle.AspNetCore.Annotations;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApi.AuthEndpoints;

public class AuthenticateEndpoint : EndpointBaseAsync
    .WithRequest<AuthenticateRequest>
    .WithActionResult<AuthenticateResponse>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenClaimsService _tokenClaimsService;

    public AuthenticateEndpoint(SignInManager<ApplicationUser> signInManager,
        ITokenClaimsService tokenClaimsService)
    {
        _signInManager = signInManager;
        _tokenClaimsService = tokenClaimsService;
    }

    [HttpPost($"{ApiConstants.API_PREFIX}/authenticate")]
    [SwaggerOperation(
        Summary = "Authenticates a user",
        Description = "Authenticates a user",
        OperationId = "auth.authenticate",
        Tags = new[] { "AuthEndpoints" })
    ]
    public override async Task<ActionResult<AuthenticateResponse>> HandleAsync(AuthenticateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = new AuthenticateResponse(request.CorrelationId());

        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

        response.Result = result.Succeeded;
        response.IsLockedOut = result.IsLockedOut;
        response.IsNotAllowed = result.IsNotAllowed;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

        if (result.Succeeded)
        {
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
        }

        return response;
    }
}
