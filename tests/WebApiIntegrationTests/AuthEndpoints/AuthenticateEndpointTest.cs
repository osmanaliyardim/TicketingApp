using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using TicketingApp.ApplicationCore.Constants;
using TicketingApp.WebApi.AuthEndpoints;
using TicketingApp.WebApi.Constants;
using TicketingApp.ApplicationCore.Extensions;

namespace TicketingApp.WebApiIntegrationTests.AuthEndpoints;

public class AuthenticateEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthenticateEndpointTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("customer@ticketing.com", AuthorizationConstants.DEFAULT_PASSWORD, true)]
    [InlineData("demouser@ticketing.com", "demopassword", false)]
    [InlineData("baduser@ticketing.com", "badpassword", false)]
    public async Task ReturnsExpectedResultGivenCredentials(string testUsername, string testPassword, bool expectedResult)
    {
        var request = new AuthenticateRequest()
        {
            Username = testUsername,
            Password = testPassword
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{ApiConstants.API_PREFIX}/authenticate", jsonContent);
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = stringResponse.FromJson<AuthenticateResponse>();

        Assert.Equal(expectedResult, model!.Result);
    }
}
