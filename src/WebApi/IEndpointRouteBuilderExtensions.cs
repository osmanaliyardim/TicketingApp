﻿namespace TicketingApp.WebApi;

public static class IEndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this WebApplication builder)
    {
        var scope = builder.Services.CreateScope();

        var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.AddRoute(builder);
        }
    }
}
