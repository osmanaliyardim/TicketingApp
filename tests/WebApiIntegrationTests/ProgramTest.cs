using Microsoft.AspNetCore.Mvc.Testing;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.PublicApiIntegrationTests;

public class ProgramTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _application;

    public ProgramTest(WebApplicationFactory<Program> factory)
    {
        _application = factory;
    }

    public HttpClient NewClient => _application.CreateClient();
}
