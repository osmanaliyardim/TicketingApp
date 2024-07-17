using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using TicketingApp.WebApi.OrderEndpoints;
using TicketingApp.WebApi.CartEndpoints;
using System.Text.Json;
using TicketingApp.WebApi.Constants;

namespace TicketingApp.WebApiIntegrationTests.OrderEndpoints;

public class CreateOrderEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CreateOrderEndpointTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Admins should not give orders
    [Fact]
    public async Task ReturnsNotAuthorizedGivenAdminUserToken()
    {
        var jsonContent = GetValidNewItemJson();
        var adminToken = ApiTokenHelper.GetAdminUserToken();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var response = await _client.PutAsync(ApiConstants.API_PREFIX + "/orders/carts/{cartId}/book", jsonContent);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // Customers should be able to give orders
    [Fact]
    public async Task ReturnsSuccessGivenValidOrderAndCustomerUserToken()
    {
        var jsonContent = GetValidNewItemJson();
        var customerToken = ApiTokenHelper.GetCustomerUserToken();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", customerToken);
        var response = await _client.PutAsync(ApiConstants.API_PREFIX + "/orders/carts/{cartId}/book", jsonContent);
        response.EnsureSuccessStatusCode();
        
        var stringResponse = await response.Content.ReadAsStringAsync();

        Assert.Empty(stringResponse);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private StringContent GetValidNewItemJson()
    {
        var request = new CreateOrderRequest()
        {
            Cart = new CartDto { Id = 10, BuyerId = "1", Items = { new CartItemDto { Id = 1, EventId = 1, EventName = "Concert X", Quantity = 2, UnitPrice = 10 } } }
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        return jsonContent;
    }
}
