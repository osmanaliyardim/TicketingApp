﻿namespace TicketingApp.WebApi;

public interface IEndpoint
{
    void AddRoute(IEndpointRouteBuilder app);
}

public interface IEndpoint<TResult> : IEndpoint
{
    Task<TResult> HandleAsync();
}

public interface IEndpoint<TResult, TRequest> : IEndpoint
{
    Task<TResult> HandleAsync(TRequest request);
}

public interface IEndpoint<TResult, TRequest1, TRequest2> : IEndpoint
{
    Task<TResult> HandleAsync(TRequest1 request1, TRequest2 request2);
}

public interface IEndpoint<TResult, TRequest1, TRequest2, TRequest3> : IEndpoint
{
    Task<TResult> HandleAsync(TRequest1 request1, TRequest2 request2, TRequest3 request3);
}
